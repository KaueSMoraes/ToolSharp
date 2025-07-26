using Core.Extensions.Tasks;
using TaskExtensions = Core.Extensions.Tasks.TaskExtensions;

namespace Tests.Extensions
{
    public class TaskExtensionsTests
    {
        [Fact]
        public async Task Then_Chains_Async_Functions()
        {
            Task<int> A() => Task.FromResult(2);
            Task<int> B(int x) => Task.FromResult(x * 3);

            var result = await A().Then(B);
            Assert.Equal(6, result);
        }

        [Fact]
        public async Task Then_Chains_To_Void_Task()
        {
            int captured = 0;
            Task<int> A() => Task.FromResult(4);
            Task SetValue(int x) => Task.Run(() => captured = x);

            await A().Then(SetValue);
            Assert.Equal(4, captured);
        }

        [Fact]
        public async Task Then_Transforms_Result_With_Sync_Function()
        {
            Task<int> A() => Task.FromResult(10);
            int Double(int x) => x * 2;

            var result = await A().Then(Double);
            Assert.Equal(20, result);
        }

        [Fact]
        public async Task Tap_Executes_Side_Effect_And_Passes_Through()
        {
            int log = 0;
            Task<int> A() => Task.FromResult(5);

            var result = await A().Tap(x => log = x * 10);
            Assert.Equal(5, result);
            Assert.Equal(50, log);
        }

        [Fact]
        public async Task If_Applies_Function_Conditionally_True()
        {
            Task<int> A() => Task.FromResult(8);
            var result = await A().If(x => x > 5, x => x * 2);
            Assert.Equal(16, result);
        }

        [Fact]
        public async Task If_Applies_Function_Conditionally_False()
        {
            Task<int> A() => Task.FromResult(3);
            var result = await A().If(x => x > 5, x => x * 2);
            
            Assert.Equal(3, result);
        }

        [Fact]
        public async Task Try_Returns_Successful_Result()
        {
            Task<int> A() => Task.FromResult(100);
            var result = await A().Try();

            Assert.True(result.IsSuccess);
            Assert.Equal(100, result.Value);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task Try_Returns_Error_Result()
        {
            Task<int> Failing() => Task.FromException<int>(new Exception("fail"));
            var result = await Failing().Try();

            Assert.False(result.IsSuccess);
            Assert.Equal(default(int), result.Value);
            Assert.NotNull(result.Error);
        }

        [Fact]
        public async Task Retry_Succeeds_After_Retries()
        {
            int attempts = 0;

            Task<int> SometimesFails() => Task.Run(() =>
            {
                attempts++;
                if (attempts < 3) throw new Exception("fail");
                return 77;
            });

            var result = await TaskExtensions.Retry(SometimesFails, retries: 5);
            Assert.Equal(77, result);
            Assert.Equal(3, attempts);
        }

        [Fact]
        public async Task Retry_Throws_If_All_Attempts_Fail()
        {
            Func<Task<int>> AlwaysFails = () => Task.FromException<int>(new Exception("fail"));
            await Assert.ThrowsAsync<InvalidOperationException>(() => TaskExtensions.Retry(AlwaysFails, retries: 2));
        }

        [Fact]
        public async Task Retry_Throws_If_Retries_Is_Less_Than_One()
        {
            Task<int> Any() => Task.FromResult(0);
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => TaskExtensions.Retry(Any, 0));
        }

        [Fact]
        public async Task Catch_Returns_Fallback_Value_On_Exception()
        {
            Task<int> Failing() => Task.FromException<int>(new InvalidOperationException("fail"));
            var result = await Failing().Catch(_ => 42);
            Assert.Equal(42, result);
        }

        [Fact]
        public async Task Catch_Returns_Fallback_Task_On_Exception()
        {
            Task<int> Failing() => Task.FromException<int>(new InvalidOperationException("fail"));
            var result = await Failing().Catch(_ => Task.FromResult(99));
            Assert.Equal(99, result);
        }

        [Fact]
        public async Task Ensure_Throws_If_Condition_Fails()
        {
            Task<int> A() => Task.FromResult(0);
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                A().Ensure(x => x > 0, "Must be greater than zero"));
        }

        [Fact]
        public async Task Then_With_Action_T_SideEffect_Returns_Result()
        {
            int value = 0;
            Task<int> A() => Task.FromResult(42);
            var result = await A().Then(x => { value = x * 2; }); 
            Assert.Equal(42, result);
            Assert.Equal(84, value);
        }


        [Fact]
        public async Task Then_With_Action_FireAndForget_Returns_Original_Result()
        {
            bool called = false;
            Task<int> A() => Task.FromResult(123);
            var result = await A().Then(() => called = true);
            Assert.True(called);
            Assert.Equal(123, result);
        }

        [Fact]
        public async Task Then_Task_Chains_TaskU_Without_Input()
        {
            Task A() => Task.CompletedTask;
            Task<int> B() => Task.FromResult(10);
            var result = await A().Then(B);
            Assert.Equal(10, result);
        }

        [Fact]
        public async Task Ensure_Returns_Result_If_Condition_Passes()
        {
            Task<int> A() => Task.FromResult(10);
            var result = await A().Ensure(x => x > 5, "Should not throw");
            Assert.Equal(10, result);
        }

        [Fact]
        public async Task Finally_Executes_After_Task_And_Returns_Result()
        {
            bool finalized = false;
            Task<int> A() => Task.FromResult(100);

            var result = await A().Finally(() => finalized = true);

            Assert.Equal(100, result);
            Assert.True(finalized);
        }

        [Fact]
        public async Task Finally_Throws_If_Final_Action_Fails()
        {
            Task<int> A() => Task.FromResult(5);
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                A().Finally(() => throw new InvalidOperationException("final error")));
        }

        [Fact]
        public async Task ThrowIf_Throws_When_Condition_Is_True()
        {
            Task<int> A() => Task.FromResult(0);
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                A().ThrowIf(x => x == 0, x => new ArgumentOutOfRangeException("Zero is invalid")));
        }

        [Fact]
        public async Task ThrowIf_Returns_Result_When_Condition_Is_False()
        {
            Task<int> A() => Task.FromResult(1);
            var result = await A().ThrowIf(x => x == 0, x => new Exception("Should not happen"));
            Assert.Equal(1, result);
        }
    }
}

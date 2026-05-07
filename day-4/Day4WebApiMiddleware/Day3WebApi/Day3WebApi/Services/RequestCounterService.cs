namespace Day3WebApi.Services
{
    public class RequestCounterService
    {
        private int _counter;

        public void Increment()
        {
            _counter++;
        }

        public int Counter => _counter;
    }
}

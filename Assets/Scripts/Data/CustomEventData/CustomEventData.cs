namespace Test.Data
{
    public sealed class CustomEventData
    {
        private readonly GameEventName _message;

        public CustomEventData(GameEventName message, object data)
        {
            Value = data;
            _message = message;
        }

        public CustomEventData(GameEventName message)
        {
            _message = message;
        }

        public object Value { get; }
        public GameEventName Message => _message;
    }
}
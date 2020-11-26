using System.Collections.Generic;
using System.Linq;
using Test.Data;
using Test.Interfaces;

namespace Test.Tools
{
    public class Publisher: IPublisher
    {
        #region Private Data

        private Dictionary<GameEventName, List<ISubscriber>> _dictionarySubscribers;

        #endregion

        public Publisher()
        {
            Initialization();
        }

        #region Public Methods
        
        public void Publish(CustomEventData messageData)
        {
            var eventName = messageData.Message;
            if (!_dictionarySubscribers.ContainsKey(eventName)) return;
        
            foreach (var subscriber in _dictionarySubscribers[eventName].ToList())
            {
                subscriber.OnEvent(messageData);
            }
        }

        public void AddSubscriber(GameEventName eventName, ISubscriber subscriber)
        {
            if (_dictionarySubscribers.ContainsKey(eventName))
            {
                if (!_dictionarySubscribers[eventName].Contains(subscriber))
                {
                    _dictionarySubscribers[eventName].Add(subscriber);
                }
            }
            else
            {
                var newSubscribersList = new List<ISubscriber> {subscriber};
                _dictionarySubscribers.Add(eventName, newSubscribersList);
            }
        }

        public void RemoveSubscriber(GameEventName eventName, ISubscriber subscriber)
        {
            if (!_dictionarySubscribers.ContainsKey(eventName)) return;
            if (_dictionarySubscribers[eventName].Contains(subscriber))
            {
                _dictionarySubscribers[eventName].Remove(subscriber);
            }
        }
        
        #endregion

        #region Private Methods

        private void Initialization()
        {
            _dictionarySubscribers = new Dictionary<GameEventName, List<ISubscriber>>();
        }

        #endregion
    }
}
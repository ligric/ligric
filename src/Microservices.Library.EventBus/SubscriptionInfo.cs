using System;

namespace Microservices.Library.EventBus
{
    public partial class InMemoryEventBusSubscriptionsManager
    {

        /// <summary>
        /// Class that describes the subscription information
        /// </summary>
        public class SubscriptionInfo
        {
            /// <summary>
            /// Defines if subscription is dynamic
            /// </summary>
            public bool IsDynamic { get; }

            /// <summary>
            /// Defines the handler type
            /// </summary>
            public Type HandlerType { get; }

            // Private constructor that defines subscription dynamic status and type handler
            private SubscriptionInfo(bool isDynamic, Type handlerType)
            {
                IsDynamic = isDynamic;
                HandlerType = handlerType;
            }

            /// <summary>
            /// Returns a new instance of a dynamic subscription
            /// </summary>
            /// <param name="handlerType"></param>
            /// <returns></returns>
            public static SubscriptionInfo Dynamic(Type handlerType)
            {
                return new SubscriptionInfo(true, handlerType);
            }

            /// <summary>
            /// Returns a new instance of a typed subscription
            /// </summary>
            /// <param name="handlerType"></param>
            /// <returns></returns>
            public static SubscriptionInfo Typed(Type handlerType)
            {
                return new SubscriptionInfo(false, handlerType);
            }
        }
    }
}

using Moq;
using SharpTwitch.Core.Enums;
using SharpTwitch.EventSub.Client;
using SharpTwitch.EventSub.Core.EventArgs;
using SharpTwitch.EventSub.Core.EventArgs.Channel.Redemption;
using SharpTwitch.EventSub.Core.EventArgs.Channel.Reward;
using SharpTwitch.EventSub.Core.EventArgs.Stream;
using SharpTwitch.EventSub.Core.EventArgs.User;
using SharpTwitch.EventSub.Core.EventMessageArgs;
using SharpTwitch.EventSub.Core.Handler;
using SharpTwitch.EventSub.Core.Models;
using SharpTwitch.EventSub.Core.Models.Stream;
using SharpTwitch.EventSub.Core.Models.User;
using SharpTwitch.EventSub.Core.SubscriptionTypes.Channel.Redemption;
using SharpTwitch.Helix.Models.Channel.Reward;
using System.Text.Json;

namespace SharpTwitch.EventSub.Tests
{
    public class EventSubTests
    {
        private readonly Mock<IWebSocketClient> _mockWebSocketClient;
        private readonly Mock<INotificationHandler> _mockNotificationHandler;

        public EventSubTests()
        {
            _mockWebSocketClient = new Mock<IWebSocketClient>();
            _mockNotificationHandler = new Mock<INotificationHandler>();
        }

        [Fact]
        public async Task EventSub_RaiseEvent_StreamOnline()
        {
            ConfigureNotificationHandler<StreamOnline, StreamOnlineArgs>(SubscriptionType.STREAM_ONLINE);
            await using var eventSub = new EventSub(new[] { _mockNotificationHandler.Object }, _mockWebSocketClient.Object);

            bool called = false;
            void handler(object? s, StreamOnlineArgs e) => called = true;

            eventSub.OnStreamOnline += handler;

            _mockWebSocketClient.Raise(w => w.OnDataMessage += null, new DataMessageArgs
            {
                Message = @"{
                    ""metadata"": { 
                        ""message_type"": ""NOTIFICATION"", 
                        ""subscription_type"": ""STREAM_ONLINE"" 
                    },
                    ""payload"": { 
                        ""event"": {} 
                    }
                }"
            });

            eventSub.OnStreamOnline -= handler;

            Assert.True(called);
        }

        [Fact]
        public async Task EventSub_RaiseEvent_StreamOffline()
        {
            ConfigureNotificationHandler<StreamOffline, StreamOfflineArgs>(SubscriptionType.STREAM_OFFLINE);
            await using var eventSub = new EventSub(new[] { _mockNotificationHandler.Object }, _mockWebSocketClient.Object);

            bool called = false;
            void handler(object? s, StreamOfflineArgs e) => called = true;

            eventSub.OnStreamOffline += handler;

            _mockWebSocketClient.Raise(w => w.OnDataMessage += null, new DataMessageArgs
            {
                Message = @"{
                    ""metadata"": { 
                        ""message_type"": ""NOTIFICATION"", 
                        ""subscription_type"": ""STREAM_OFFLINE"" 
                    },
                    ""payload"": { 
                        ""event"": {} 
                    }
                }"
            });

            eventSub.OnStreamOffline -= handler;

            Assert.True(called);
        }

        [Fact]
        public async Task EventSub_RaiseEvent_UserUpdate()
        {
            ConfigureNotificationHandler<UserUpdate, UserUpdateArgs>(SubscriptionType.USER_UPDATE);
            await using var eventSub = new EventSub(new[] { _mockNotificationHandler.Object }, _mockWebSocketClient.Object);

            bool called = false;
            void handler(object? s, UserUpdateArgs e) => called = true;

            eventSub.OnUserUpdate += handler;

            _mockWebSocketClient.Raise(w => w.OnDataMessage += null, new DataMessageArgs
            {
                Message = @"{
                    ""metadata"": { 
                        ""message_type"": ""NOTIFICATION"", 
                        ""subscription_type"": ""USER_UPDATE"" 
                    },
                    ""payload"": { 
                        ""event"": {} 
                    }
                }"
            });

            eventSub.OnUserUpdate -= handler;

            Assert.True(called);
        }

        [Fact]
        public async Task EventSub_RaiseEvent_AddCustomReward()
        {
            ConfigureNotificationHandler<CustomReward, CustomRewardAddArgs>(SubscriptionType.CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_ADD);
            await using var eventSub = new EventSub(new[] { _mockNotificationHandler.Object }, _mockWebSocketClient.Object);

            bool called = false;
            void handler(object? s, CustomRewardAddArgs e) => called = true;

            eventSub.OnCustomRewardAdd += handler;

            _mockWebSocketClient.Raise(w => w.OnDataMessage += null, new DataMessageArgs
            {
                Message = @"{
                    ""metadata"": { 
                        ""message_type"": ""NOTIFICATION"", 
                        ""subscription_type"": ""CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_ADD"" 
                    },
                    ""payload"": { 
                        ""event"": {} 
                    }
                }"
            });

            eventSub.OnCustomRewardAdd -= handler;

            Assert.True(called);
        }

        [Fact]
        public async Task EventSub_RaiseEvent_UpdateCustomReward()
        {
            ConfigureNotificationHandler<CustomReward, CustomRewardUpdateArgs>(SubscriptionType.CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_UPDATE);
            await using var eventSub = new EventSub(new[] { _mockNotificationHandler.Object }, _mockWebSocketClient.Object);

            bool called = false;
            void handler(object? s, CustomRewardUpdateArgs e) => called = true;

            eventSub.OnCustomRewardUpdate += handler;

            _mockWebSocketClient.Raise(w => w.OnDataMessage += null, new DataMessageArgs
            {
                Message = @"{
                    ""metadata"": { 
                        ""message_type"": ""NOTIFICATION"", 
                        ""subscription_type"": ""CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_UPDATE"" 
                    },
                    ""payload"": { 
                        ""event"": {} 
                    }
                }"
            });

            eventSub.OnCustomRewardUpdate -= handler;

            Assert.True(called);
        }

        [Fact]
        public async Task EventSub_RaiseEvent_RemoveCustomReward()
        {
            ConfigureNotificationHandler<CustomReward, CustomRewardRemoveArgs>(SubscriptionType.CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_REMOVE);
            await using var eventSub = new EventSub(new[] { _mockNotificationHandler.Object }, _mockWebSocketClient.Object);

            bool called = false;
            void handler(object? s, CustomRewardRemoveArgs e) => called = true;

            eventSub.OnCustomRewardRemove += handler;

            _mockWebSocketClient.Raise(w => w.OnDataMessage += null, new DataMessageArgs
            {
                Message = @"{
                    ""metadata"": { 
                        ""message_type"": ""NOTIFICATION"", 
                        ""subscription_type"": ""CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_REMOVE"" 
                    },
                    ""payload"": { 
                        ""event"": {} 
                    }
                }"
            });

            eventSub.OnCustomRewardRemove -= handler;

            Assert.True(called);
        }

        [Fact]
        public async Task EventSub_RaiseEvent_AddCustomRewardRedemption()
        {
            ConfigureNotificationHandler<ChannelPointsCustomRewardRedemption, CustomRewardRedemptionArgs>(SubscriptionType.CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_REDEMPTION_ADD);
            await using var eventSub = new EventSub(new[] { _mockNotificationHandler.Object }, _mockWebSocketClient.Object);

            bool called = false;
            void handler(object? s, CustomRewardRedemptionArgs e) => called = true;

            eventSub.OnChannelPointsCustomRewardRedemption += handler;

            _mockWebSocketClient.Raise(w => w.OnDataMessage += null, new DataMessageArgs
            {
                Message = @"{
                    ""metadata"": { 
                        ""message_type"": ""NOTIFICATION"", 
                        ""subscription_type"": ""CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_REDEMPTION_ADD"" 
                    },
                    ""payload"": { 
                        ""event"": {} 
                    }
                }"
            });

            eventSub.OnChannelPointsCustomRewardRedemption -= handler;

            Assert.True(called);
        }

        #region Helpers
        private void ConfigureNotificationHandler<T, K>(SubscriptionType subscriptionType)
            where T : class
            where K : EventSubEventArgs<EventSubMessage<EventPayload<T>>>
        {
            _mockNotificationHandler.Setup(x => x.SubscriptionType).Returns(subscriptionType);
            _mockNotificationHandler.Setup(x => x.Raise(It.IsAny<EventSubBase>(), It.IsAny<JsonDocument>(), It.IsAny<JsonSerializerOptions>()))
                .Callback<EventSubBase, JsonDocument, JsonSerializerOptions>((eventSubBase, jsonDocument, options) =>
                {
                    var notification = new EventSubMessage<EventPayload<T>>();
                    K eventArgs = (K)Activator.CreateInstance(typeof(K), notification)!;
                    eventSubBase.RaiseEvent(subscriptionType, eventArgs);
                });
        }
        #endregion
    }
}
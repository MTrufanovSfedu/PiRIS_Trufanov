using DbService;
using DbService.Objects;
using Grpc.Core;

namespace GrpcService.Services
{
    public class Work3Service(ILogger<Work3Service> logger) : Work3.Work3Base
    {
        private DatabaseService database = new DatabaseService();
        private int userId = 0;

        public override Task<AuthUserReply> AuthUser(AuthUserRequest request, ServerCallContext context)
        {
            logger.LogInformation("Auth request for {UserName}", request.User.UserName);

            int reply = 2;
            StoreUser storeUser = database.GetUserByUserName(request.User.UserName);

            if (storeUser == null)
            { 
                reply = 1;
                logger.LogInformation("Auth request denied for {UserName}: No such user", request.User.UserName);
            }

            if (storeUser != null && storeUser.UserPassword == request.User.Password)
            {
                reply = 0;
                logger.LogInformation("{UserName} has logged in", request.User.UserName);
            }

            if (reply == 2) logger.LogInformation("Auth request denied for {UserName}: Incorrect password", request.User.UserName);

            return Task.FromResult(new AuthUserReply
            {
                Reply = reply
            });
        }
        public override Task<CreateUserReply> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            logger.LogInformation("Create request for {UserName}", request.User.UserName);
            int reply = 0;
            StoreUser storeUser = database.GetUserByUserName(request.User.UserName);

            if (storeUser == null)
            {
                while (database.GetUserById(userId) != null) userId++;
                StoreUser newUser = new StoreUser();
                newUser.Id = userId;
                newUser.UserName = request.User.UserName;
                newUser.UserPassword = request.User.Password;
                if (database.CreateUser(newUser) != true) reply = 2;
            }

            if (storeUser != null)
            {
                reply = 1;
                logger.LogInformation("Create request denied for {UserName}: Already exists", request.User.UserName);
            }

            if (reply == 2) logger.LogInformation("Create request failed for {UserName}", request.User.UserName);

            if (reply == 0) logger.LogInformation("{UserName} has been created", request.User.UserName);

            return Task.FromResult(new CreateUserReply
            {
                Reply = reply
            });
        }

        public override Task<GetItemReply> GetItem(GetItemRequest request, ServerCallContext context)
        {
            logger.LogInformation("Add item {Id} request", request.Item.Id);
            int reply = 0;
            StorePosition storePosition = database.GetPositionById(((int)request.Item.Id));

            // Новый элемент
            if (storePosition == null)
            {
                StorePosition newPosition = new StorePosition();
                newPosition.Id = ((int)request.Item.Id);
                newPosition.PositionName = request.Item.PositionName;
                newPosition.PositionType = request.Item.PositionType;
                newPosition.PositionValue = ((int)request.Item.PositionValue);
                newPosition.PositionPrice = request.Item.PositionPrice;
                newPosition.PriceCurrency = request.Item.PriceCurrency;
                if (database.AddPosition(newPosition) != true) reply = 1;
            }

            // Изменить существующий элемент
            if (storePosition != null)
            {
                database.DeletePosition(((int)request.Item.Id));
                StorePosition newPosition = new StorePosition();
                newPosition.Id = ((int)request.Item.Id);
                newPosition.PositionName = request.Item.PositionName;
                newPosition.PositionType = request.Item.PositionType;
                newPosition.PositionValue = ((int)request.Item.PositionValue);
                newPosition.PositionPrice = request.Item.PositionPrice;
                newPosition.PriceCurrency = request.Item.PriceCurrency;
                if (database.AddPosition(newPosition) != true) reply = 2;
            }

            if (reply == 0 && storePosition == null) 
            {
                logger.LogInformation("Item {Id} has been added", request.Item.Id);
            }

            if (reply == 0 && storePosition != null)
            {
                logger.LogInformation("Item {Id} has been edited", request.Item.Id);
            }

            return Task.FromResult(new GetItemReply
            {
                Reply = reply
            });
        }
        public override Task<SendItemReply> SendItem(SendItemRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get item {Id} request", request.Id);
            int reply = 0;
            GrpcService.Item item = new GrpcService.Item();
            StorePosition storePosition = database.GetPositionById(((int)request.Id));

            if (storePosition == null)
            {
                reply = 1;
                logger.LogInformation("Get item {Id} request failed: Item not found", request.Id);
            }

            if (storePosition != null)
            {
                item.Id = storePosition.Id;
                item.PositionName = storePosition.PositionName;
                item.PositionType = storePosition.PositionType;
                item.PositionValue = storePosition.PositionValue;
                item.PositionPrice = storePosition.PositionPrice;
                item.PriceCurrency = storePosition.PriceCurrency;
                logger.LogInformation("Item {Id} has been sent", request.Id);
            }

            return Task.FromResult(new SendItemReply
            {
                Reply = reply,
                Item = item
            });
        }
        public override Task<SendAllItemsReply> SendAllItems(SendAllItemsRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get all items request");
            SendAllItemsReply reply = new SendAllItemsReply();
            reply.Reply = 0;

            List<StorePosition> storePositions = database.GetPositions();

            if (storePositions == null) 
            {
                logger.LogInformation("Get all items request failed");
                reply.Reply = 1;
            }

            if (storePositions != null)
            {
                while (storePositions.Count > 0)
                {
                    GrpcService.Item item1 = new GrpcService.Item();
                    StorePosition item2 = storePositions.First();
                    item1.Id = item2.Id;
                    item1.PositionName = item2.PositionName;
                    item1.PositionType = item2.PositionType;
                    item1.PositionValue = item2.PositionValue;
                    item1.PositionPrice = item2.PositionPrice;
                    item1.PriceCurrency = item2.PriceCurrency;
                    reply.Item.Add(item1);
                    storePositions.RemoveAt(0);
                }
                logger.LogInformation("All items have been sent");
            }

            return Task.FromResult(reply);
        }
    }
}

using System.Linq;
using CodeWarriors.API.Controllers;
using CodeWarriors.BLL.Logic;
using Faker;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;

namespace CodeWarriors.API.Tests.Logic
{
    [TestClass]
    public class FriendBLLTests
    {

        [TestMethod]
        [Fact]
        public void Send_Friend_Request_Accept_As_Friend_Remove_Friend_Test()
        {
            var repo = new FriendBLL(new UserBLL());

            var controller = new UserController(new UserBLL());

            var userList = controller.GetAllUsers(NumberFaker.Number(1, 10), NumberFaker.Number(1, 3),
                ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul"));

            //Get two user to make friend
            var userId = userList.Select(r => r.Id).First();

            var friendId = userList.Where(r => !r.Id.Equals(userId)).Select(r => r.Id).First();

            //Test whether thay are friend or not

            var alreadyFriend = repo.IsExistsFriend(userId, friendId);

            if (alreadyFriend)
            {
                // They are friend so try to find who are not friend
                //userId = userList.Where(r => r.Id != userId).Select(r => r.Id).First();

                //friendId = userList.Where(r => r.Id != userId && r.Id != friendId).Select(r => r.Id).First();

                // Remove friend 
                var removeThemFriend = repo.RemoveFriend(userId, friendId);

                if (TestingConfig.XUnit)
                    Xunit.Assert.True(removeThemFriend);
                else
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(removeThemFriend);

                alreadyFriend = repo.IsExistsFriend(userId, friendId);
            }

            if (TestingConfig.XUnit)
                Xunit.Assert.False(alreadyFriend);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(alreadyFriend);

            // Send friend request
            var test = repo.SendFriendRequest(userId, friendId);

            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);

            // Now one send friend request to other  
            alreadyFriend = repo.IsExistsFriend(userId, friendId);

            if (TestingConfig.XUnit)
                Xunit.Assert.True(alreadyFriend);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(alreadyFriend);

            /*
            // Get request object to accept
            var friend = repo.GetFriendRequest(userId, friendId);

            if (TestingConfig.XUnit)
                Xunit.Assert.NotNull(friend);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(friend);

            var testFriendIds = friend.UserId == userId && friend.FriendId == friendId;

            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);

            // Check friend request has been pending
            var testPendingFriendRequest = friend.IsAccepted;

            if (TestingConfig.XUnit)
                Xunit.Assert.False(testPendingFriendRequest);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(testPendingFriendRequest);
            */

            // Check all request of a user
            var friendRequests = repo.GetAllFriendRequestsByUser(userId);

            friendRequests = repo.GetAllFriendRequestsByUser(friendId);

            if (TestingConfig.XUnit)
                Xunit.Assert.NotNull(friendRequests);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(friendRequests);

            var testFriendRequests = friendRequests.Count() > 1;

            if (TestingConfig.XUnit)
                Xunit.Assert.True(testFriendRequests);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(testFriendRequests);

            // Accept a request and make friend 
            var acceptFriendRequest = repo.AcceptFriendRequest(userId, friendId);

            if (TestingConfig.XUnit)
                Xunit.Assert.True(acceptFriendRequest);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(acceptFriendRequest);

            /*
            var friendInfo = repo.GetFriendRequest(userId, friendId);

            if (TestingConfig.XUnit)
                Xunit.Assert.NotNull(friendInfo);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(friendInfo);

            // check friend requested accepted
            var testAcceptedFriendRequest = friend.IsAccepted;

            if (TestingConfig.XUnit)
                Xunit.Assert.True(testAcceptedFriendRequest);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(testAcceptedFriendRequest);

            */

            // Get all friend of a user
            var friends = repo.GetAllFriendsByUser(userId);

            if (TestingConfig.XUnit)
                Xunit.Assert.NotNull(friends);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(friends);

            var testFriends = friends.Count() > 1;

            if (TestingConfig.XUnit)
                Xunit.Assert.True(testFriends);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(testFriends);

            // Remove friend 
            var removeFriend = repo.RemoveFriend(userId, friendId);

            if (TestingConfig.XUnit)
                Xunit.Assert.True(removeFriend);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(removeFriend);

            // They are not friend any more  
            var notFriend = repo.IsExistsFriend(userId, friendId);

            if (TestingConfig.XUnit)
                Xunit.Assert.False(notFriend);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(notFriend);

        }

        [TestMethod]
        [Fact]
        public void Send_And_Reject_Friend_Request_Test()
        {
            var repo = new FriendBLL(new UserBLL());

            var controller = new UserController(new UserBLL());

            var userList = controller.GetAllUsers(NumberFaker.Number(1, 10), NumberFaker.Number(1, 3),
                ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul"));

            //Get two user to make friend
            var userId = userList.Select(r => r.Id).First();

            var friendId = userList.Where(r => r.Id != userId).Select(r => r.Id).First();

            //Test whether thay are friend or not

            var alreadyFriend = repo.IsExistsFriend(userId, friendId);

            if (alreadyFriend)
            {
                // They are friend so try to find who are not friend
                //userId = userList.Where(r => r.Id != userId).Select(r => r.Id).First();

                //friendId = userList.Where(r => r.Id != userId && r.Id != friendId).Select(r => r.Id).First();

                // Remove friend 
                var removeThemFriend = repo.RemoveFriend(userId, friendId);

                if (TestingConfig.XUnit)
                    Xunit.Assert.True(removeThemFriend);
                else
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(removeThemFriend);

                alreadyFriend = repo.IsExistsFriend(userId, friendId);
            }

            if (TestingConfig.XUnit)
                Xunit.Assert.False(alreadyFriend);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(alreadyFriend);

            // Send friend request
            var test = repo.SendFriendRequest(userId, friendId);

            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);

            // Now one send friend request to other  
            alreadyFriend = repo.IsExistsFriend(userId, friendId);

            if (TestingConfig.XUnit)
                Xunit.Assert.True(alreadyFriend);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(alreadyFriend);

            /*
            // Get request object to reject
            var friend = repo.GetFriendRequest(userId, friendId);

            if (TestingConfig.XUnit)
                Xunit.Assert.NotNull(friend);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(friend);

            var testFriendIds = friend.UserId == userId && friend.FriendId == friendId;

            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);

            // Check friend request has been pending
            var testPendingFriendRequest = friend.IsAccepted;

            if (TestingConfig.XUnit)
                Xunit.Assert.False(testPendingFriendRequest);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(testPendingFriendRequest);
            */

            // Check all request of a user
            var friendRequests = repo.GetAllFriendRequestsByUser(friendId);

            friendRequests = repo.GetAllFriendRequestsByUser(userId);

            if (TestingConfig.XUnit)
                Xunit.Assert.NotNull(friendRequests);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(friendRequests);

            var testFriendRequests = friendRequests.Count() > 1;

            if (TestingConfig.XUnit)
                Xunit.Assert.True(testFriendRequests);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(testFriendRequests);

            // Reject a request and make friend 
            var rejectFriendRequest = repo.RejectFriendRequest(userId, friendId);

            if (TestingConfig.XUnit)
                Xunit.Assert.True(rejectFriendRequest);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(rejectFriendRequest);

            /*
            // Now no friend request
            var friendInfo = repo.GetFriendRequest(userId, friendId);

            if (TestingConfig.XUnit)
                Xunit.Assert.Null(friendInfo);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNull(friendInfo);
            */

            // The are not friend  
            alreadyFriend = repo.IsExistsFriend(userId, friendId);

            if (TestingConfig.XUnit)
                Xunit.Assert.False(alreadyFriend);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(alreadyFriend);

        }

        [TestMethod]
        [Fact]
        public void Find_Mutual_Friend_Test()
        {
            var repo = new FriendBLL(new UserBLL());

            var controller = new UserController(new UserBLL());

            var userList = controller.GetAllUsers(NumberFaker.Number(1, 10), NumberFaker.Number(1, 2),
                ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul"));

            //Get three user to make friend
            var userId = userList.Select(r => r.Id).First();

            var friendId = userList.Where(r => r.Id != userId).Select(r => r.Id).First();

            //Test whether thay are friend or not

            var alreadyFriend = repo.IsExistsFriend(userId, friendId);

            if (!alreadyFriend)
            {
                // Make them friend
                repo.SendFriendRequest(userId, friendId);
                repo.AcceptFriendRequest(userId, friendId);
                alreadyFriend = repo.IsExistsFriend(userId, friendId);
            }

            if (TestingConfig.XUnit)
                Xunit.Assert.True(alreadyFriend);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(alreadyFriend);

            var mutualFriend = userList.Where(r => r.Id != userId && r.Id != friendId).Select(r => r.Id).First();

            //Test whether thay are friend or not

            alreadyFriend = repo.IsExistsFriend(mutualFriend, friendId);

            if (!alreadyFriend)
            {
                // Make them friend
                repo.SendFriendRequest(mutualFriend, friendId);
                repo.AcceptFriendRequest(mutualFriend, friendId);
                alreadyFriend = repo.IsExistsFriend(mutualFriend, friendId);
            }

            if (TestingConfig.XUnit)
                Xunit.Assert.True(alreadyFriend);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(alreadyFriend);

            //Test whether thay are friend or not

            alreadyFriend = repo.IsExistsFriend(mutualFriend, userId);

            if (!alreadyFriend)
            {
                // Make them friend
                repo.SendFriendRequest(mutualFriend, userId);
                repo.AcceptFriendRequest(mutualFriend, userId);
                alreadyFriend = repo.IsExistsFriend(mutualFriend, userId);
            }

            if (TestingConfig.XUnit)
                Xunit.Assert.True(alreadyFriend);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(alreadyFriend);

            // Get Mutual friend -> Mr X and Mr Y found Mr. Z

            var mutualFriends = repo.GetMutualFriend(userId, mutualFriend);

            if (TestingConfig.XUnit)
                Xunit.Assert.NotNull(mutualFriends);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(mutualFriends);

            var test = mutualFriends.Count() > 0;

            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);

            // Get Mutual friend -> Mr Y and Mr X found Mr. Z

            mutualFriends = repo.GetMutualFriend(mutualFriend, userId);

            if (TestingConfig.XUnit)
                Xunit.Assert.NotNull(mutualFriends);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(mutualFriends);

            test = mutualFriends.Count() > 0;

            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);

            // Get Mutual friend -> Mr Y and Mr Z found Mr. X

            mutualFriends = repo.GetMutualFriend(mutualFriend, friendId);

            if (TestingConfig.XUnit)
                Xunit.Assert.NotNull(mutualFriends);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(mutualFriends);

            test = mutualFriends.Count() > 0;

            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);

            // Get Mutual friend -> Mr Z and Mr Y found Mr. X

            mutualFriends = repo.GetMutualFriend(friendId, mutualFriend);

            if (TestingConfig.XUnit)
                Xunit.Assert.NotNull(mutualFriends);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(mutualFriends);

            test = mutualFriends.Count() > 0;

            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);

            // Get Mutual friend -> Mr Z and Mr X found Mr. Y

            mutualFriends = repo.GetMutualFriend(friendId, userId);

            if (TestingConfig.XUnit)
                Xunit.Assert.NotNull(mutualFriends);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(mutualFriends);

            test = mutualFriends.Count() > 0;

            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);

            // Get Mutual friend -> Mr X and Mr Z found Mr. Y

            mutualFriends = repo.GetMutualFriend(friendId, userId);

            if (TestingConfig.XUnit)
                Xunit.Assert.NotNull(mutualFriends);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(mutualFriends);

            test = mutualFriends.Count() > 0;

            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);

        }

        [TestMethod]
        [Fact]
        public void Friend_Suggestion_Test()
        {
            var repo = new FriendBLL(new UserBLL());

            var controller = new UserController(new UserBLL());

            var userList = controller.GetAllUsers(NumberFaker.Number(1, 10), NumberFaker.Number(1, 2),
                ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul"));

            //Get Seven user to make friend
            var userIds = userList.Select(r => r.Id).Take(7).ToList();

            var userId = userIds.First();

            var friendId = userIds.Skip(1).First();

            //Test whether thay are friend or not
            var alreadyFriend = repo.IsExistsFriend(userId, friendId);

            if (!alreadyFriend)
            {
                // Make them friend
                repo.SendFriendRequest(userId, friendId);
                repo.AcceptFriendRequest(userId, friendId);
                alreadyFriend = repo.IsExistsFriend(userId, friendId);
            }

            if (TestingConfig.XUnit)
                Xunit.Assert.True(alreadyFriend);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(alreadyFriend);

            friendId = userIds.Skip(2).First();

            //Test whether thay are friend or not
            alreadyFriend = repo.IsExistsFriend(userId, friendId);

            if (!alreadyFriend)
            {
                // Make them friend
                repo.SendFriendRequest(userId, friendId);
                repo.AcceptFriendRequest(userId, friendId);
                alreadyFriend = repo.IsExistsFriend(userId, friendId);
            }

            if (TestingConfig.XUnit)
                Xunit.Assert.True(alreadyFriend);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(alreadyFriend);

            friendId = userIds.Skip(3).First();

            //Test whether thay are friend or not
            alreadyFriend = repo.IsExistsFriend(userId, friendId);

            if (!alreadyFriend)
            {
                // Make them friend
                repo.SendFriendRequest(userId, friendId);
                repo.AcceptFriendRequest(userId, friendId);
                alreadyFriend = repo.IsExistsFriend(userId, friendId);
            }

            if (TestingConfig.XUnit)
                Xunit.Assert.True(alreadyFriend);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(alreadyFriend);


            friendId = userIds.Skip(4).First();

            //Test whether thay are friend or not
            alreadyFriend = repo.IsExistsFriend(userId, friendId);

            if (!alreadyFriend)
            {
                // Make them friend
                repo.SendFriendRequest(userId, friendId);
                repo.AcceptFriendRequest(userId, friendId);
                alreadyFriend = repo.IsExistsFriend(userId, friendId);
            }

            if (TestingConfig.XUnit)
                Xunit.Assert.True(alreadyFriend);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(alreadyFriend);

            userId = userIds.Skip(1).First();

            friendId = userIds.Skip(2).First();

            //Test whether thay are friend or not
            alreadyFriend = repo.IsExistsFriend(userId, friendId);

            if (!alreadyFriend)
            {
                // Make them friend
                repo.SendFriendRequest(userId, friendId);
                repo.AcceptFriendRequest(userId, friendId);
                alreadyFriend = repo.IsExistsFriend(userId, friendId);
            }

            if (TestingConfig.XUnit)
                Xunit.Assert.True(alreadyFriend);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(alreadyFriend);

            userId = userIds.Skip(5).First();

            //Test whether thay are friend or not
            alreadyFriend = repo.IsExistsFriend(userId, friendId);

            if (!alreadyFriend)
            {
                // Make them friend
                repo.SendFriendRequest(userId, friendId);
                repo.AcceptFriendRequest(userId, friendId);
                alreadyFriend = repo.IsExistsFriend(userId, friendId);
            }

            if (TestingConfig.XUnit)
                Xunit.Assert.True(alreadyFriend);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(alreadyFriend);

            friendId = userIds.Skip(3).First();

            //Test whether thay are friend or not
            alreadyFriend = repo.IsExistsFriend(userId, friendId);

            if (!alreadyFriend)
            {
                // Make them friend
                repo.SendFriendRequest(userId, friendId);
                repo.AcceptFriendRequest(userId, friendId);
                alreadyFriend = repo.IsExistsFriend(userId, friendId);
            }

            if (TestingConfig.XUnit)
                Xunit.Assert.True(alreadyFriend);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(alreadyFriend);

            userId = userIds.Skip(4).First();

            friendId = userIds.Skip(6).First();

            //Test whether thay are friend or not
            alreadyFriend = repo.IsExistsFriend(userId, friendId);

            if (!alreadyFriend)
            {
                // Make them friend
                repo.SendFriendRequest(userId, friendId);
                repo.AcceptFriendRequest(userId, friendId);
                alreadyFriend = repo.IsExistsFriend(userId, friendId);
            }

            if (TestingConfig.XUnit)
                Xunit.Assert.True(alreadyFriend);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(alreadyFriend);

            userId = userIds.First();

            // Get Suggested Friend of 'A' should found 'C' and 'G'

            var suggestedFriends = repo.GetFriendSuggestion(userId);

            if (TestingConfig.XUnit)
                Xunit.Assert.NotNull(suggestedFriends);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(suggestedFriends);

            var test = suggestedFriends.Count() > 0;

            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);

            var suggestedFriendIds = suggestedFriends.Select(r => r.UserId.Equals(userId) ? r.FriendId : r.UserId);

            test = suggestedFriendIds.Contains(userIds.Skip(5).First());

            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);

            test = suggestedFriendIds.Contains(userIds.Skip(6).First());

            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);

        }

    }
}

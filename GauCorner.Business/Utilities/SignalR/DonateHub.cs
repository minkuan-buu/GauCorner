using Microsoft.AspNetCore.SignalR;

public class DonateHub : Hub
{
    public async Task JoinGroup(Guid userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
    }

    public async Task SendDonateNotification(Guid userId, DonateNotification data)
    {
        await Clients.Group(userId.ToString()).SendAsync("ReceiveDonateNotification", data);
    }
}
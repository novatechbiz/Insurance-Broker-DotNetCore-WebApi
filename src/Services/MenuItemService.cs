using InsuraNova.Models;

namespace InsuraNova.Services
{
    public interface IMenuItemService
    {
        List<MenuItem> GetMenuItems();
    }

    public class MenuItemService : IMenuItemService
    {
        public List<MenuItem> GetMenuItems()
        {
            return new List<MenuItem>
            {
                new MenuItem { Title = "Home", Path = "/dashboards/home", Icon = "home" },
                new MenuItem { Title = "Clients", Path = "/dashboards/clients", Icon = "users" },
                new MenuItem { Title = "Reports", Path = "/dashboards/reports", Icon = "document" },
                new MenuItem { Title = "Policies", Path = "/dashboards/policies", Icon = "shield" },
                new MenuItem { Title = "Setting", Path = "/dashboards/setting", Icon = "settings" },
                new MenuItem { Title = "Support", Path = "/dashboards/support", Icon = "support" }
            };
        }
    }
}

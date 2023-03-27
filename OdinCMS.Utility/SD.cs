using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdinCMS.Utility
{
    public static class SD
    {
        /* Customer roles */
        public const string Role_User_Individual = "Individual";
        public const string Role_User_Company = "Company";

        /* Manage roles */
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";

        /* Order status */
        public const string Order_Pending = "Pending";
        public const string Order_Approved = "Approved";
        public const string Order_Processing = "Processing";
        public const string Order_Shipped = "Shipped";
        public const string Order_Cancelled = "Cancelled";
        public const string Order_Refunded = "Refunded";

        /* Payment status */
        public const string Payment_Pending = "Pending";
        public const string Payment_Approved = "Approved";
        public const string Payment_ApprovedForDelayedPayment = "ApprovedForDelayedPayment";
        public const string Payment_Rejected = "Rejected";


    }
}

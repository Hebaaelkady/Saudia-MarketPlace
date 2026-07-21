using System;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Infrastructure.Shared.Implementations
{
    public class Keys : IKeys
    {
        public string EncryptionMasterKey() => "r4u7x!A%D*G-KaPdRgUkXp2s5v8y/B?E";
        public string Admin()
        {
            return "71f1c28b-d6b9-4739-8a74-98c";
        }
        public string frontEndUserRole()
        {
            return "f932102f-1307-4d32-9e1d-5713f81f71450d";
        }
        public string storeRole()
        {
            return "h932102f-1307-4d32-9e1d-55713f81f71450d";
        }
        public string InsideShippingRole()
        {
            return "vv2102f-1307-4d32-9e1d-55713f81f71450d";
        }
        public string ManagOrderRole()
        {
            return "22102f-1307-4d32-9e1d-55713f81f71450d";
        }
        public string UsersRole()
        {
            return "tt2102f-1307-4d32-9e1d-55713f81f71450d";
        }
        public string ShippingRole()
        {
            return "k732102f-1307-4d32-9e1d-55713f81f71450d";
        }
        public string ProductRole()
        {
            return "j82102f-1307-4d32-9e1d-55713f81f71450d";
        }
        public string ReportRole()
        {
            return "v92102f-1307-4d32-9e1d-55713f81f71450d";
        }
        public string MainRole()
        {
            return "l62102f-1307-4d32-9e1d-55713f81f71450d";
        }
        public string OtherPagesRole()
        {
            return "dd2102f-1307-4d32-9e1d-55713f81f71450d";
        }
        public string CatogryRole()
        {
            return "xx2102f-1307-4d32-9e1d-55713f81f71450d";
        }
        public int AssignToStore()
        {
            return 12;
        }
        public int AssignedToShipping()
        {
            return 13;
        }
        public int AssignedToInsideShipping()
        {
            return 14;
        }
        public int newTiket()
        {
            return 1;
        }
         
        public int TransportMethodByme()
        {
            return 2;
        }
        public string TransportMethodByme_text()
        {
            return "بواسطتي";
        }
        public int TransportMethodByCompany()
        {
            return 1;
        }
        public string TransportMethodByCompany_text()
        {
            return "عن طريق شركة شحن";
        }
        public int CatType_gomla()
        {
            return 1;
        }
        public int CatType_qta3a()
        {
            return 2;
        }
        public string CatType_gomla_text()
        {
            return "جملة";
        }
        public string CatType_qta3a_text()
        {
            return "قطاعي";
        }
        public string URL_cat()
        {
            return "Catogry/catogry";
        }
       
        public string URL_product()
        {
            return "Product/Product";
        }
        public string URL_Edit_Product()
        {
            return "Product/Edit_Product";
        }
        public string URL_Edit_Product_qta3a()
        {
            return "Product/Edit_Product_qta3a";
        }
        public string URL_register()
        {
            return "Account/Register";
        }
        public string URL_CreateProduct_gomla()
        {
            return "Product/CreateProduct_gomla";
        }
        public string URL_CreateProduct_qta3a()
        {
            return "Product/URL_CreateProduct_qta3a";
        }
        public string URL_EditShippingPrice()
        {
            return "Product/EditShippingPrice";
        }
        public string URL_DeleteProduct()
        {
            return "Product/DeleteProduct";
        }
        public string URL_DeleteShippingPrice()
        {
            return "Product/DeleteShippingPrice";
        }
        public string URL_Product_gomla_details()
        {
            return "Product/Product_gomla_details";
        }
        public string URL_AddCatogry()
        {
            return "Catogry_gomla/AddCatogry";
        }
        public string URL_Catogry()
        {
            return "Catogry_gomla/Catogry";
        }
        public string URL_AddCatogryqt3()
        {
            return "Catogry_qta3a/AddCatogry";
        }
        public string URL_Catogryqt3()
        {
            return "Catogry_qta3a/Catogry";
        }
        public string URL_Product_qt3()
        {
       return "Product/Product_qta3a";
        }
        public string URL_Product_gomla()
        {     return "Product/Product_gomla";
            
        }
        
        public string URL_GetproductByCat()
        {
            return "Product/GetproductByCat";
        }
        public string URL_GetproductByCat_qta3a()
        {
            return "Product/GetproductByCat_qta3a";
        }
        public string URL_NewOrders()
        {
            return "Orders/NewOrders";
        }
        public string URL_OrderDetails()
        {
            return "Orders/OrderDetails";
        }
        public string URL_UnderCheck()
        {
            return "Orders/UnderCheck";
        }
        public string URL_AddRolegomla()
        {
            return "Catogry/AddRolegomla";
        }
        public string URL_AddRoleqta3a()
        {
            return "Catogry/AddRoleqta3a";
        }

        public string URL_Product_qta3a_details()
        {
            return "Product/Product_qta3a_details";
        }
        
        public string URL_product_qta3a()
        {
            return "Product/Product_qta3a_Added";
        }
        
       
        public int TypeUser_backend()
        {
            return 1;
        }
        public int TypeUser_frontend()
        {
            return 2;
        }
       
        public int MorningShift()
        {
            return 0;
        }
        public int NightShift()
        {
            return 1;
        }
        public string MorningShiftText()
        {
            return "من 9صباحا الي 3 مساء ";
        }
        public string NightShiftText()
        {
            return "من 5صباحا الي 10 مساء ";
        }

        public int status_check()
        {
            return 3;
        }
    }
}

using System;

namespace Kader.Infrastructure.Shared.Interfaces
{
    public interface IKeys
    {
        string EncryptionMasterKey();
        string storeRole();
        string ManagOrderRole();
        string ShippingRole();
        string UsersRole();
        string InsideShippingRole();
        string ProductRole();
        string ReportRole();
        int AssignedToInsideShipping();
        string MainRole();
        string OtherPagesRole();
        string CatogryRole();
        int AssignToStore();
        int newTiket();
        int AssignedToShipping();
        int TransportMethodByme();
        int  TransportMethodByCompany();
        string TransportMethodByme_text();
        string TransportMethodByCompany_text();
        int CatType_gomla();
        int CatType_qta3a();
        string CatType_gomla_text();
        string CatType_qta3a_text();
        public string URL_cat(); 
        public string URL_product();
        public string URL_register();
        public string URL_CreateProduct_gomla();
        public string URL_CreateProduct_qta3a();
        public string URL_AddRolegomla();
        public string URL_GetproductByCat();
        public string URL_Edit_Product();
        public string URL_EditShippingPrice();
        public string URL_DeleteProduct();
        public string URL_DeleteShippingPrice();
        public string URL_Product_gomla_details();
        public string URL_AddRoleqta3a();
        public string URL_Product_qta3a_details();
        public string URL_Edit_Product_qta3a();
        public string URL_product_qta3a(); 
        public string Admin();
        public int TypeUser_backend();
        string frontEndUserRole();
    public int TypeUser_frontend();
        public int MorningShift();
        public int NightShift();
        public string MorningShiftText();
        public string NightShiftText();
        public int status_check();
        public string URL_AddCatogry();
        public string URL_Catogry();
        public string URL_AddCatogryqt3();
        public string URL_Catogryqt3();
        public string URL_Product_qt3();
        public string URL_Product_gomla();
        public string URL_NewOrders();
        public string URL_OrderDetails();
        public string URL_UnderCheck();
    }
}

//Sayfalarda ki Img metotları bunu kullanıyor.
export const Base =
  process.env.NODE_ENV === "production" ? "https://api.test.com/api/" : "http://127.0.0.1:5103/api/";

//ProductService > GetMiracleWorld metodu bu yapıyı kullanıyor.
export const CoreBase =
  process.env.NODE_ENV === "production" ? "https://apicore.test.com/api/" : "http://127.0.0.1:5101/api/";

const Authentication = Base + "authentication/";
const Account = Base + "account/";
const News = Base + "news/";
const Product = Base + "product/";
const CoreProduct = CoreBase + "product/";
const About = Base + "about/";
const ContactForm = Base + "contactForm/";
const ContactInfo = Base + "contactInfo/";
const FeedBack = Base + "feedBack/";
const LiveTicket = Base + "liveTicket/";
const Market = Base + "market/";
const Slider = Base + "slider/";
const SmtpSetting = Base + "smtpSetting/";
const User = Base + "user/";
const Ticket = Base + "ticket/";
const VersionInfo = Base + "versionInfo/";
const Purchase = Base + "purchase/";

export const ControllerRoutes = {
  About: About,
  Account: Account,
  Authentication: Authentication,
  ContactForm: ContactForm,
  ContactInfo: ContactInfo,
  FeedBack: FeedBack,
  LiveTicket: LiveTicket,
  Market: Market,
  News: News,
  Product: Product,
  User: User,
  Slider: Slider,
  SmtpSetting: SmtpSetting,
  Ticket: Ticket,
  VersionInfo: VersionInfo,
  Purchase: Purchase,
};

export const CRUDRoutes = {
  ListAll(controller) {
    return controller + "listAll";
  },
  List(controller) {
    return controller + "list";
  },
  Create(controller) {
    return controller + "create";
  },
  Update(controller, objectId) {
    return controller + `update/${objectId}`;
  },
  Delete(controller, objectId) {
    return controller + `delete/${objectId}`;
  },
  GetById(controller, objectId) {
    return controller + `get/${objectId}`;
  },
  GetCount(controller) {
    return controller + "count";
  },
};

export const AccountRoutes = {
  ChangePassword() {
    return Account + "changePass";
  },
  ForgotPassword() {
    return Account + "forgotPassword";
  },
  ResetPasswordOutSource() {
    return Account + "resetpass";
  },
  Register() {
    return Account + "register";
  },
};

export const AuthenticateRoutes = {
  Authenticate() {
    return Authentication + "authenticate";
  },
  AuthenticateByRefreshToken() {
    return Authentication + "authenticateByRefreshToken";
  },
  RevokeAuthenticate() {
    return Authentication + "revokeAuthenticate";
  },
};

export const UserRoutes = {
  GetProfileImage() {
    return User + "getProfileImageOutSource";
  },
  GetOutSource() {
    return User + "getoutsource";
  },
  UpdateOutSource() {
    return User + "updateoutsource";
  },
};

export const ProductRoutes = {
  GetByTag(tag) {
    return Product + `getByTag/${tag}`;
  },
  GetProducts() {
    return Product + "getProducts";
  },
  GetImage(productId) {
    return Product + `getImage/${productId}`;
  },
  GetMiracleWorld(platform) {
    return CoreProduct + `getMiracleWorld/${platform}`;
  },
  AddProductDetail(productId) {
    return Product + `addProductDetail/${productId}`;
  },
  RemoveProductDetail(productId, productDetailId) {
    return Product + `removeProductDetail/${productId}/${productDetailId}`;
  },
};

export const NewsRoutes = {
  GetByTag(tag) {
    return News + `getByTag/${tag}`;
  },
  GetImage(imageId) {
    return News + `getImage/${imageId}`;
  },
};

export const AboutRoutes = {
  Get() {
    return About + "get";
  },
  GetImage() {
    return About + "getImage";
  },
};

export const SliderRoutes = {
  GetImage(sliderId) {
    return Slider + `getImage/${sliderId}`;
  },
};

export const MarketRoutes = {
  GetCurrency() {
    return Market + "getCurrency";
  },
  ReadyToPurchase() {
    return Market + "readyToPurchase";
  },
  PurchaseOrder() {
    return Market + "purchase";
  },
};

export const TicketRoutes = {
  GetImage(ticketMessageId) {
    return Ticket + `getImage/${ticketMessageId}`;
  },
  GetUserInfo(userId) {
    return Ticket + `getUserInfo/${userId}`;
  },
  GetMessageGroups() {
    return Ticket + "getMessageGroups";
  },
  GetTicketMessages(ticketGroupId) {
    return Ticket + `getTicketMessages/${ticketGroupId}`;
  },
  GetGroupReadInfo(groupId) {
    return Ticket + `getGroupReadInfo/${groupId}`;
  },
  GetGroupCloseInfo(groupId) {
    return Ticket + `getGroupCloseInfo/${groupId}`;
  },
  IsClosedChange(groupId) {
    return Ticket + `isClosedChange/${groupId}`;
  },
};

export const LiveTicketRoutes = {
  GetContents(roomName) {
    return LiveTicket + `getContents/${roomName}`;
  },
  GetChats(roomName) {
    return LiveTicket + `getChats/${roomName}`;
  },
  GetCustomerName(roomName){
    return LiveTicket + `getCustomerName/${roomName}`
  }
};

export const FeedBackRoutes = {
  GetOptions(){
    return FeedBack + "getOptions";
  }
}

import Vue from "vue";
import VueRouter from "vue-router";

//Layouts
import IndexLayout from "./layouts/IndexLayout";
import AdminLayout from "./layouts/AdminLayout";
import UserLayout from "./layouts/UserLayout";
import EmptyLayout from "./layouts/EmptyLayout";
import QTLayout from "./layouts/QTLayout";

//Account Pages
import Login from "./pages/Account/Login";
import Register from "./pages/Account/Register";
import ForgotPasswordRequest from "./pages/Account/ForgotPasswordRequest";
import ForgotPasswordResponse from "./pages/Account/ForgotPasswordResponse";

//Web Pages
import Home from "./pages/Home/Home";
import News from "./pages/News/News";
import ProductDetails from "./pages/Product/ProductDetails";
import Contact from "./pages/Contact/Contact";
import About from "./pages/About/About";
import Market from "./pages/Market/Market";
import MiracleWorld from "./pages/Market/MiracleWorld";
import Order from "./pages/Market/Order";
import LiveTicket from "./pages/LiveTicket/LiveTicket";
import Usage from "./pages/Usage/Usage";
import FeedBack from "./pages/FeedBack/FeedBack";

//Admin Pages
import AdminHome from "./admin/pages/Home/AdminHome";
import ContactsManager from "./admin/pages/Contact/Contacts";
import NewsManager from "./admin/pages/News/News";
import SlidersManager from "./admin/pages/Slider/Sliders";
import ProductsManager from "./admin/pages/Product/Products";
import SettingsManager from "./admin/pages/Settings/Settings";
import PurchasesManager from "./admin/pages/Purchase/Purchases";
import TicketsManager from "./admin/pages/Tickets/TicketResponse";
import TicketMessageDetailsAdmin from "./admin/pages/Tickets/components/TicketMessageDetails";
import LiveTicketManager from "./admin/pages/LiveTicket/LiveTicketManager";

//User Pages
import UserHome from "./user/pages/Home/UserHome";
import UserProducts from "./user/pages/Product/Products";
import UserProfile from "./user/pages/Settings/Profile";
import TicketRequest from "./user/pages/Ticket/TicketRequest";
import TicketMessageDetails from "./user/pages/Ticket/components/TicketMessageDetails";
import Viewer from "./user/pages/Views/View";

//QT Pages
import NewsQT from "./qt/pages/News/NewsQT";
import MarketQT from "./qt/pages/Market/MarketQT";
import OrderQT from "./qt/pages/Market/OrderQT";

Vue.use(VueRouter);

export const routes = [
  //Page Routes
  {
    path: "/account", component: EmptyLayout,
    children: [
      { path: "login", component: Login, name: "login", meta: { requiresAuth: false } },
      { path: "register", component: Register, name: "register", meta: { requiresAuth: false } },
      { path: "forgot-password-req", component: ForgotPasswordRequest, name: "resetPass", meta: { requiresAuth: false } },
      { path: "forgot-password-res/:code", component: ForgotPasswordResponse, name: "changePass", meta: { requiresAuth: false } },
    ],
  },

  {
    path: "/", component: IndexLayout, meta: { requiresAuth: false },
    children: [
      { path: "/", component: Home, name: "home", meta: { requiresAuth: false } },


      { path: "news", component: News, name: "news", meta: { requiresAuth: false } },
      { path: "product-details/tag=:tag", component: ProductDetails, name: "product-details", meta: { requiresAuth: false } },
      { path: "about", component: About, name: "about", meta: { requiresAuth: false } },
      { path: "contact", component: Contact, name: "contact", meta: { requiresAuth: false } },
      { path: "market", component: Market, name: "market", meta: { requiresAuth: false } },
      { path: "miracle-world", component: MiracleWorld, name: "miracle-world", meta: { requiresAuth: false } },
      { path: "order", component: Order, name: "order", meta: { requiresAuth: false } },
      { path: "chat", component: LiveTicket, name: "chat", meta: { requiresAuth: false } },
      { path: "usage", component: Usage, name: "usage", meta: { requiresAuth: true, requiresAdmin: true } },
      { path: "feedback", component: FeedBack, name: "feedback", meta: { requiresAuth: true, requiresAdmin: false } },
    ],
  },
  {
    path: "/admin",
    component: AdminLayout, meta: { requiresAuth: true, requiresAdmin: true },
    children: [
      { path: "/admin", component: AdminHome, name: "adminHome", meta: { requiresAuth: true, requiresAdmin: true } },
      { path: "news", component: NewsManager, name: "newsManager", meta: { requiresAuth: true, requiresAdmin: true } },
      { path: "products", component: ProductsManager, name: "productManager", meta: { requiresAuth: true, requiresAdmin: true } },
      { path: "contacts", component: ContactsManager, name: "contactsManager", meta: { requiresAuth: true, requiresAdmin: true } },
      { path: "sliders", component: SlidersManager, name: "sliderManager", meta: { requiresAuth: true, requiresAdmin: true } },
      { path: "settings", component: SettingsManager, name: "settingsManager", meta: { requiresAuth: true, requiresAdmin: true } },
      { path: "purchases", component: PurchasesManager, name: "purchaseManager", meta: { requiresAuth: true, requiresAdmin: true } },
      { path: "ticket-res", component: TicketsManager, name: "ticketsManager", meta: { requiresAuth: true, requiresAdmin: true } },
      { path: "ticket-details/groupId=:groupId&userId=:userId", component: TicketMessageDetailsAdmin, name: "ticket-details", meta: { requiresAuth: true } },
      { path: "ticket-live", component: LiveTicketManager, name: "ticketLive", meta: { requiresAuth: true, requiresAdmin: true } },
    ],
  },
  {
    path: "/user", component: UserLayout, meta: { requiresAuth: true },
    children: [
      { path: "/user", component: UserHome, name: "userHome", meta: { requiresAuth: true } },
      { path: "profile", component: UserProfile, name: "profileManager", meta: { requiresAuth: true } },
      { path: "products", component: UserProducts, name: "products", meta: { requiresAuth: true } },
      { path: "ticket-req", component: TicketRequest, name: "ticket", meta: { requiresAuth: true } },
      { path: "ticket-details", component: TicketMessageDetails, name: "ticket-details", meta: { requiresAuth: true } },
      { path: "view", component: Viewer, name: "view", meta: { requiresAuth: true } },
    ],
  },
  {
    path: "/qt", component: QTLayout,
    children: [
      { path: "news/tag=:tag", component: NewsQT, name: "news-qt", meta: { requiresAuth: false } },
      { path: "market", component: MarketQT, name: "market-qt", meta: { requiresAuth: false } },
      { path: "order", component: OrderQT, name: "order-qt", meta: { requiresAuth: false } },
      { path: "product-details/tag=:tag", component: ProductDetails, name: "product-details-qt", meta: { requiresAuth: false } },
    ],
  },
  { path: "*", redirect: "/" },
];

export const router = new VueRouter({
  mode: "history",
  routes,
  scrollBehavior(to, from, savedPosition) {
    if (savedPosition) {
      return savedPosition;
    } else {
      return { x: 0, y: 0 };
    }
  },
});

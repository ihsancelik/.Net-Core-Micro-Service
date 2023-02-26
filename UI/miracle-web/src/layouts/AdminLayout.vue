<template>
  <v-app id="inspire" style="background-color: #7f7f7f">
    <v-app-bar app clipped-right color="#37474f" dark fluid>
      <v-app-bar-nav-icon @click.stop="left = !left" />
      <v-row justify="end">
        <h2>
          <router-link :style="routerLink" tag="a" to="/">
            <a style="color: white">Miracle</a>
          </router-link>
        </h2>
      </v-row>
      <v-spacer />

      <v-btn icon color="#767676" class="ma-2 white--text" to="/" target="_blank">
        <v-icon left dark>mdi-home</v-icon>
      </v-btn>

      <v-divider class="ml-3 mr-3 v-divider--vertical"></v-divider>

      <language-switcher id="langSwitcher" />
      <v-divider class="ml-3 mr-3 v-divider--vertical"></v-divider>
      <v-btn icon color="#767676" class="ma-2 white--text" @click="logout()">
        <v-icon left dark>mdi-logout</v-icon>
      </v-btn>
    </v-app-bar>

    <v-navigation-drawer
      v-model="left"
      fixed
      color="#37474f"
      mobile-breakpoint="700"
      bottom
      mini-variant
      mini-variant-width="100"
    >
      <template>
        <div class="text-center" v-for="(route, index) in navigationRoutes" :key="index">
          <template>
            <v-menu v-if="route.parent" top :offset-x="true">
              <template #activator="{ on }">
                <a v-on="on" class="a0 d-flex flex-column">
                  <v-icon color="white" large>{{ route.icon }}</v-icon>
                  {{ translator(route.name) }}
                </a>
              </template>
              <v-list color="#37474f">
                <v-list-item v-for="(item, index) in route.subRoutes" :key="index" id="a1">
                  <v-list-item-title>
                    <router-link :style="routerLink" tag="a" :to="{ name: item.link }">
                      <v-btn style="color: white; font-size: small; width: 100%" text>
                        {{ translator(item.name) }}
                      </v-btn>
                    </router-link>
                  </v-list-item-title>
                </v-list-item>
              </v-list>
            </v-menu>
            <router-link v-else :style="routerLink" tag="a" :to="{ name: route.link }">
              <a class="a0 d-flex flex-column">
                <v-icon color="white" large>{{ route.icon }}</v-icon>
                {{ translator(route.name) }}
              </a>
            </router-link>
          </template>
        </div>
      </template>
    </v-navigation-drawer>

    <v-content>
      <v-container fluid>
        <v-row justify="center" class="justify-md-center">
          <v-fade-transition mode="out-in">
            <router-view />
          </v-fade-transition>
        </v-row>
        <v-col cols="12" md="5"> </v-col>
      </v-container>
    </v-content>

    <v-footer app color="rgb(75, 91, 99)" class="white--text">
      <v-spacer />
      <h4>
        <router-link :style="routerLink" tag="a" to="/">
          <a style="color: white">&copy; A.I.T. SOFTWARE 2020</a>
        </router-link>
      </h4>
    </v-footer>
  </v-app>
</template>

<script>
import LanguageSwitcher from "../components/Header/LanguageSwitcher";
import AccountService from "../services/AccountService";

export default {
  data() {
    return {
      drawer: null,
      left: false,

      selectedProduct: {},
      dialogEnable: false,
      dialogKey: 0,
      dialogWidth: null,
      selectedComponent: "",
      componentProps: null,
      openIcon: "./admin-chat.jpg",

      navigationRoutes: [
        {
          name: "home",
          icon: "mdi-home",
          link: "adminHome",
          parent: false,
          subRoutes: null,
        },
        {
          name: "purchases",
          icon: "mdi-shopping",
          link: "purchaseManager",
          parent: false,
          subRoutes: null,
        },
        {
          name: "products",
          icon: "mdi-application",
          link: "productManager",
          parent: false,
          subRoutes: null,
        },
        {
          name: "news",
          icon: "mdi-newspaper-plus",
          link: "newsManager",
          parent: false,
          subRoutes: null,
        },
        {
          name: "sliders",
          icon: "mdi-play-box-outline",
          link: "sliderManager",
          parent: false,
          subRoutes: null,
        },
        {
          name: "contacts",
          icon: "mdi-email",
          link: "contactsManager",
          parent: false,
          subRoutes: null,
        },
        {
          name: "ticket",
          icon: "mdi-account-switch",
          link: null,
          parent: true,
          subRoutes: [
            { name: "ticketsManager", link: "ticketsManager" },
            { name: "liveTicketManager", link: "ticketLive" },
          ],
        },
        {
          name: "settings",
          icon: "mdi-cogs",
          link: "settingsManager",
          parent: false,
          subRoutes: null,
        },
      ],
      routerLink: {
        textDecorationLine: "none !important",
      },
    };
  },
  components: {
    LanguageSwitcher,
  },
  methods: {
    async logout() {
      let response = await new AccountService().logout();
      if (response) this.$router.push("/account/login").catch(() => {});
    },
  },
};
</script>

<style>
.a0 {
  font-family: sans-serif !important;
  font-size: small !important;
  color: white !important;
  margin: 10px 0 !important;
  border-bottom: 1px solid #666666 !important;
  align-content: center !important;
  justify-content: center !important;
  text-decoration-line: none !important;
}

.a0:hover {
  background: rgba(255, 255, 255, 0.08) !important;
}

#a1 {
  color: white !important;
  font-size: small;
  align-content: center;
  justify-content: center;
  padding: 0;
}

#a1:hover {
  background: rgba(255, 255, 255, 0.08);
}

#langSwitcher .v-input__slot:before {
  border-style: none !important;
}
</style>

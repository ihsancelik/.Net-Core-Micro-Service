<template>
  <v-app id="inspire" style="background-color: white">
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

      <FeedBack
        :rating="rating"
        :options="options"
        :products="products"
        :selectedOption="selectedOption"
        :selectedProduct="selectedProduct"
        :message="message"
        :dialog="dialog"
      />

      <v-btn icon color="black" class="ma-2 white--text" to="/" target="_blank">
        <v-icon left dark>mdi-home</v-icon>
      </v-btn>

      <v-divider class="ml-3 mr-3 v-divider--vertical"></v-divider>

      <language-switcher id="langSwitcher" />

      <v-divider class="ml-3 mr-3 v-divider--vertical"></v-divider>

      <v-btn icon color="white" class="ma-2 white--text" @click="logout()">
        <v-icon left dark>mdi-logout</v-icon>
      </v-btn>
    </v-app-bar>

    <v-navigation-drawer v-model="left" fixed temporary color="#37474f" width="10%">
      <template>
        <div class="text-center">
          <template>
            <router-link :style="routerLink" tag="a" :to="{ name: 'userHome' }">
              <a class="a0 d-flex flex-column">
                <v-icon color="white" large>mdi-home</v-icon>
                {{ translator("home") }}
              </a>
            </router-link>
          </template>
        </div>
      </template>

      <template>
        <div class="text-center">
          <template>
            <router-link :style="routerLink" tag="a" :to="{ name: 'profileManager' }">
              <a class="a0 d-flex flex-column">
                <v-icon color="white" large>mdi-account</v-icon>
                {{ translator("profileManager") }}
              </a>
            </router-link>
          </template>
        </div>
      </template>

      <template>
        <div class="text-center">
          <template>
            <router-link :style="routerLink" tag="a" :to="{ name: 'ticket' }">
              <a class="a0 d-flex flex-column">
                <v-icon color="white" large>mdi-account-switch</v-icon>
                {{ translator("ticketRequest") }}
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
import FeedBack from "../pages/FeedBack/FeedBack";
import ProductService from "@/services/ProductService";
import FeedBackService from "@/services/FeedBackService";

export default {
  created() {
    new ProductService().getListAll().then((productsResponse) => {
      this.products = productsResponse.list;
    });
    new FeedBackService().getOptions().then((optionsResponse) => {
      this.options = optionsResponse;
    });
  },
  data() {
    return {
      drawer: null,
      left: false,
      dialog: false,

      settings: [
        { title: "sliders", routeName: "sliderManager" },
        { title: "about", routeName: "aboutManager" },
        { title: "TicketRequest", routeName: "ticket" },
      ],
      routerLink: {
        textDecorationLine: "none !important",
      },

      products: [],
      options: [],
      rating: 4,
      selectedOption: "",
      selectedProduct: "",
      message: "",
    };
  },
  components: {
    LanguageSwitcher,
    FeedBack,
  },
  methods: {
    async logout() {
      let response = await new AccountService().logout();
      if (response) this.$router.push("account/login").catch(() => {});
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

#langSwitcher .v-input__slot:before {
  border-style: none !important;
}

.input {
  margin: 1% 6%;
  width: -webkit-fill-available;
  height: 70px;
  border-radius: 2px;
}

::placeholder {
  color: rgb(165, 165, 218);
  font-family: Arial, Helvetica, sans-serif;
  font-size: 16px;
}

input:focus,
button:focus {
  outline: none;
}
</style>
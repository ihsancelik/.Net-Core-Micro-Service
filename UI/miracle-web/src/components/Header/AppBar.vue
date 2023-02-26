<template>
  <div>
    <v-app-bar id="home-app-bar" app color="white" elevation="1" height="80">
      <v-toolbar-title>
        <v-img src="/logo.png" />
      </v-toolbar-title>
      <v-spacer />

      <div>
        <v-tabs class="hidden-sm-and-down" optional>
          <v-tab
            v-for="(item, i) in menus"
            :key="i"
            :to="item.to"
            :exact="item.name === 'home'"
            :ripple="false"
            active-class="text--primary"
            class="font-weight-bold"
            min-width="96"
            text
          >
            {{ translator(item.name) }}
          </v-tab>
          <v-tab :to="accountPanel.link">
            {{ accountPanel.name }}
          </v-tab>

          <cart class="appBarButton" />

          <v-tab>
            <language-switcher id="langSwitcher" />
          </v-tab>
        </v-tabs>
      </div>

      <v-app-bar-nav-icon class="hidden-md-and-up" @click="drawer = !drawer" />
    </v-app-bar>

    <home-drawer v-model="drawer" :menus="menus" />
  </div>
</template>

<script>
export default {
  components: {
    Cart: () => import("../Cart/Cart"),
    HomeDrawer: () => import("./Drawer"),
    LanguageSwitcher: () => import("./LanguageSwitcher"),
  },
  created() {
    if (localStorage.getItem("webToken") && localStorage.getItem("roles")) {
      this.accountPanel = {
        name: this.translator("account"),
        link: localStorage.getItem("roles").includes("Admin") ? "/admin" : "/user",
      };
    } else this.accountPanel = { name: this.translator("login"), link: "account/login" };
  },
  data: () => ({
    drawer: null,
    menus: [
      { name: "download", to: "miracle-world" },
      { name: "home", to: "/" },
      { name: "market", to: "market" },
      { name: "news", to: "news" },
      { name: "about", to: "about" },
      { name: "contact", to: "contact" },
    ],
    accountPanel: {},
  }),
};
</script>

<style lang="sass">
#home-app-bar
  .v-tabs-slider
    max-width: 24px
    margin: 0 auto

  .v-tab
    &::before
      display: none

#langSwitcher .v-input__slot:before
  border-style: none !important

.appBarButton
  align-items: center
  display: flex
  flex: 0 1 auto
  font-size: 0.875rem
  font-weight: 500
  justify-content: center
  letter-spacing: 0.0892857143em
  line-height: normal
  min-width: 90px
  max-width: 360px
  outline: none
  padding: 0 16px
  position: relative
  text-align: center
  text-decoration: none
  text-transform: uppercase
  transition: none
</style>

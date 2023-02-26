<template>
  <v-app id="inspire">
    <v-app-bar app style="height: 0;">
      <v-btn text @click="$router.go(-1)"><v-icon large color="primary">mdi-arrow-left-circle</v-icon></v-btn>
      <v-spacer />
      <cart class="appBarButton"/>

      <language-switcher id="langSwitcher" />
    </v-app-bar>
    <v-main>
      <v-container class="fill-height" fluid>
        <v-row justify="center" class="justify-md-center">
          <v-fade-transition mode="out-in">
            <router-view />
          </v-fade-transition>
        </v-row>
      </v-container>
    </v-main>
    <v-footer app>
      <v-spacer />
      <h4 style="color: black; user-select: none;">
        &copy; A.I.T. SOFTWARE 2020
      </h4>
    </v-footer>
  </v-app>
</template>

<script>
import LanguageSwitcher from "../components/Header/LanguageSwitcher";
import Cart from "../components/Cart/Cart";
export default {
  components: { LanguageSwitcher, Cart },
};
</script>

<style lang="sass">
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

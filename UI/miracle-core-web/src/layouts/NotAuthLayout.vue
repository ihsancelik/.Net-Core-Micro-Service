<template>
  <v-app id="inspire">
    <v-app-bar app style="height: 0;">
      <v-spacer />
      <language-switcher id="langSwitcher" />
    </v-app-bar>
    <v-main>
      <v-container class="fill-height" fluid>
        <v-row justify="center" class="justify-md-center">
          <v-fade-transition mode="out-in">
            <router-view></router-view>
          </v-fade-transition>
        </v-row>
      </v-container>
    </v-main>
    <v-footer app>
      <v-spacer />
      <h4>
        <router-link :style="routerLink" tag="a" to="/">
          <a style="color: black;">&copy; A.I.T. SOFTWARE 2020</a>
        </router-link>
      </h4>
    </v-footer>
  </v-app>
</template>

<script>
import LanguageSwitcher from "../components/LanguageSwitcher";
export default {
  data() {
    return {
      routerLink: {
        textDecorationLine: "none !important",
      },
    };
  },
  components: { LanguageSwitcher },
};
</script>

<style>
#langSwitcher .v-input__slot:before {
  border-style: none !important;
}
</style>

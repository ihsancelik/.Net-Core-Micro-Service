<template>
  <v-app id="inspire">
    <v-app-bar app style="height: 0;">
      <v-spacer />
      <language-switcher id="langSwitcher" />
    </v-app-bar>
    <v-content>
      <v-container class="fill-height" fluid>
        <v-row justify="center" class="justify-md-center">
          <v-fade-transition mode="out-in">
            <router-view />
          </v-fade-transition>
        </v-row>
      </v-container>
    </v-content>
    <v-footer app>
      <v-spacer />
      <h4>
        <router-link id="routerLink" tag="a" to="/">
          <a style="color: black;">&copy; A.I.T. SOFTWARE 2020</a>
        </router-link>
      </h4>
    </v-footer>
  </v-app>
</template>

<script>
import LanguageSwitcher from "../components/Header/LanguageSwitcher";
export default {
  components: { LanguageSwitcher },
};
</script>

<style>
#langSwitcher .v-input__slot:before {
  border-style: none !important;
}
</style>

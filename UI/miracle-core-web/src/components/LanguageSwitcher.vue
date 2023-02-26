<template>
  <v-select
    id="langSwitcher"
    @change="changeLang"
    v-model="lang"
    :items="languages"
    value="value"
    style="max-width: 50px; margin-bottom: -20px;"
  >
    <template #selection="data">
      <img width="30px" style="margin-bottom: -30px;" :alt="data.item.value" :src="'/flags/' + data.item.image" />
    </template>
    <template #item="data">
      <img width="30px" :alt="data.item.value" :src="'/flags/' + data.item.image" /> {{ data.item.text }}
    </template>
  </v-select>
</template>

<script>
export default {
  name: "LanguageSwitcher",
  data() {
    return {
      lang: "",
      languages: [
        { text: "Türkçe", value: "tr", image: "tr.svg" },
        { text: "English", value: "en", image: "en.svg" },
        { text: "Deutsch", value: "de", image: "de.svg" },
        { text: "Español", value: "es", image: "es.svg" },
        { text: "Français", value: "fr", image: "fr.svg" },
        { text: "Italiano", value: "it", image: "it.svg" },
      ],
    };
  },
  methods: {
    changeLang() {
      this.$vuetify.lang.current = this.lang;
      localStorage.setItem("language", this.lang);
      location.reload();
    },
  },
  created() {
    let lang = localStorage.getItem("language");
    this.lang = lang !== null ? lang : "en";
    this.$vuetify.lang.current = lang !== null ? lang : "en";
    localStorage.setItem("language", this.lang);
  },
};
</script>

<style></style>

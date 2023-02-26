<template>
  <div class="pt-5 pb-5 pr-2 pl-2">
    <v-container>
      <v-row>
        <v-col cols="12" md="7">
          <div class="text-justify">
            <p style="font-size: large; color: black; padding-right: 5%;">
              <span> <h3>{{ about.title }} </h3> </span
              >
              {{ about.text }}
            </p>
          </div>
        </v-col>
        <v-col cols="12" md="5">
          <v-img id="aboutImg" :src="about.img" width="100%"></v-img>
        </v-col>
      </v-row>
    </v-container>
  </div>
</template>

<script>
import { Base } from "@/helpers/RouteConstraints";
import AboutService from "@/services/AboutService";

export default {
  data() {
    return {
      about: {},
    };
  },
  methods: {
    async initialize() {
      let aboutResponse = await new AboutService().get();
      let imageResponse = await new AboutService().getImage();
      const about = aboutResponse.data;
      about.img = Base + imageResponse;
      this.about = about;
    },
  },
  created() {
    this.initialize();
  },
};
</script>

<style>
#aboutImg .v-responsive__content {
  width: 100% !important;
}
</style>

<template>
  <div style="width: 100%;">
    <v-row v-for="(n, index) in news" :key="index" class="newsRow elevation-10">
      <v-col cols="12" md="4">
        <v-layout class="d-md-block pt-5 pb-5 pr-2 pl-2">
          <vue-previewer :style="style" :images="n.img" mode="image" :options="options">
            <template #footer="{ image }">{{ image.name }}</template>
          </vue-previewer>
        </v-layout>
      </v-col>
      <v-col cols="12" md="8">
        <v-layout class="d-md-block pt-5 pb-5 pr-2 pl-2">
          <div class="headline">
            <span style="font-size: xx-large; color: black;">
              <strong>{{ n.title }}</strong>
            </span>
            <div class="subtitle-1">
              <span style="font-size: medium; vertical-align: top; color: black;">{{ n.titleDescription }}</span>
            </div>
            <div class="subtitle-2" style="margin-bottom: 20px;">
              <span style="font-size: smaller; color: black;">
                {{ n.startDate }}
              </span>
            </div>
          </div>
          <div class="text-justify">
            <p style="font-size: large; color: black;" v-html="n.text"></p>
          </div>
        </v-layout>
      </v-col>
    </v-row>
  </div>
</template>

<script>
import NewsService from "@/services/NewsService";
import { Base } from "@/helpers/RouteConstraints";

export default {
  data() {
    return {
      news: [],
      tag: "",
      options: {
        defaultWidth: "100%",
        defaultHeight: "300px",
        // more previewer options here
      },
      style: {
        width: "100%",
        // more styles here
      },
    };
  },
  methods: {
    async initialize() {
      let newsResponse = await new NewsService().getByTag({ tag: this.$route.params.tag });
      for (const news of newsResponse.list) {
        let imageResponse = await new NewsService().getImage(news.id);
        let img = [];
        img.push(Base + imageResponse);
        news.img = img;
        this.news.push(news);
      }
    },
  },
  created() {
    this.initialize();
  },
};
</script>
<style>
.newsRow {
  background: #eeeeee;
  margin: 25px 2% 25px 2%;
}
</style>

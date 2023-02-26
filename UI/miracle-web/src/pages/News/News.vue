<template>
  <div style="width: 100%;">
    <v-row v-for="(n, index) in orderedNews" :key="index" class="newsRow elevation-10" style="justify-content: center;">
      <v-col cols="12" xl="4" md="5" sm="8">
        <v-layout class="d-md-block pt-5 pb-5 pr-2 pl-2">
          <vue-previewer :style="style" :images="n.img" mode="image" :options="options">
            <template #footer="{ image }">{{ image.name }}</template>
          </vue-previewer>
        </v-layout>
      </v-col>
      <v-col cols="12" md="1"></v-col>
      <v-col cols="12" xl="6" md="6" sm="12">
        <v-layout class="d-md-block pt-5 pb-5 pr-2 pl-2">
          <div class="text-justify">
            <div class="headline">
              <span style="font-size: x-large; color: black;">
                <strong>{{ n.title }}</strong>
              </span>
              <div class="subtitle-2" style="margin-bottom: 20px;">
                <span style="font-size: smaller; color: black;">
                  {{ n.startDate }}
                </span>
              </div>
            </div>
            <p style="font-size: medium; color: black;" v-html="n.text"></p>
          </div>
        </v-layout>
      </v-col>
    </v-row>
  </div>
</template>

<script>
import { Base } from "@/helpers/RouteConstraints";
import NewsService from "@/services/NewsService";
import _ from "lodash";

export default {
  data() {
    return {
      news: [],
      options: {
        defaultWidth: "100%",
        defaultHeight: "300px",
        // more previewer options here
      },
      style: {
        width: "100%",
        // more styles here
      },
      pagedList: {},
      paginationModel: {
        page: 1,
        pageSize: -1,
      },
    };
  },
  methods: {
    async initialize() {
      let newsResponse = await new NewsService().getListAll();
      for (const news of newsResponse.list) {
        let imageResponse = await new NewsService().getImage(news.id);
        let img = [];
        img.push(Base + imageResponse);
        news.img = img;
        this.news.push(news);
      }
    },
  },
  computed: {
    orderedNews() {
      return _.orderBy(this.news, ["startDate"], ["desc"]);
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

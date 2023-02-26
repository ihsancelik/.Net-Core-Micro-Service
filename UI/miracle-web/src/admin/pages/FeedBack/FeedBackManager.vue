<template>
  <v-card style="width: 90%; padding: 2% 1%; padding: 2% 1%">
    <v-data-table
      :headers="headers"
      :items="items"
      item-key="text"
      class="elevation-1"
      :search="search"
      group-by="options"
      style="width: 80%; margin-left: 10%; padding-left: 2%"
    >
      <template v-slot:top>
        <v-text-field v-model="search" :label="translator('search')" class="mx-4"> </v-text-field>
      </template>
      <template #item.message="{ item }">
        <div :style="{ maxWidth: '600px', maxHeight: '80px', overflowY: 'scroll', padding: '15px 5px' }">
          {{ item.message }}
        </div>
      </template>
    </v-data-table>
  </v-card>
</template>

<script>
/* eslint-disable */
import FeedBackService from "@/services/FeedBackService";
export default {
  data() {
    return {
      items: [],
      headers: [
        { text: "Username", value: "userName" },
        { text: "Rate", align: "left", value: "rate" },
        { text: "Product", value: "selectedProduct" },
        { text: "Options", value: "options" },
        { text: "Message", value: "message" },
      ],
      search: "",
    };
  },
  created() {
    this.getFeedBacks();
  },
  methods: {
    getFeedBacks() {
      this.items = [];
      new FeedBackService().getListAll().then((response) => {
        console.log(response.list);
        for (const item of response.list) {
          this.items.push(item);
        }
      });
    },
  },
};
</script>
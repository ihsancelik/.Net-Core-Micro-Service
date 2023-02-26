<template>
 <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)"><v-icon style="float: right;">mdi-close-circle</v-icon></a>
    <v-layout>
      <v-data-table
        :headers="headers"
        :items="productInfoModelsProp"
        :server-items-length="pagedList.rowCount"
        :items-per-page.sync="paginationModel.pageSize"
        :page.sync="paginationModel.page"
      >
      </v-data-table>
    </v-layout>
  </v-card>
</template>

<script>
export default {
  props: ["productInfoModelsProp"],
  data() {
    return {
      usage: [],
      headers: [
        { text: this.translator("product"), value: "product" },
        { text: this.translator("totalUsageMinute"), value: "totalUsageMinute" },
        { text: this.translator("isDemo"), value: "isDemo" },
        { text: this.translator("isActive"), value: "isActive" },
        { text: this.translator("firstUsageDate"), value: "firstUsageDate" },
        { text: this.translator("lastUsageDate"), value: "lastUsageDate" },
      ],
      userInfoModels: [],
      selectedContactMessage: {},

      dialogEnable: false,
      dialogKey: 0,
      dialogWidth: null,
      selectedComponent: "",
      componentProps: null,
      pagedList: {},
      paginationModel: {
        page: 1,
        pageSize: 30,
      },

      signalModel: {
        changes: false,
        returnValues: null,
      },
      
    };
  },
  methods: {
   closeDialog() {
      this.dialogEnable = false;
      this.dialogKey += 1;
    },
    close(changes) {
      this.signalModel.changes = changes;
      this.$emit("closed", this.signalModel);
    },
  },
};
</script>

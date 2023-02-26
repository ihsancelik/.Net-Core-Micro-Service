<template>
  <v-container style="">
      <v-row> userCount: {{ usage.userCount }} </v-row>
      <v-row> ghostUserCount: {{ usage.ghostUserCount }} </v-row>
      <v-row> continuesDemoUser: {{ usage.continuesDemoUser }} </v-row>
      <v-row> totalUsageHours: {{ usage.totalUsageHours }} </v-row>
      <v-row> totalActiveUser: {{ usage.totalActiveUser }} </v-row>
     <v-row>
        <v-data-table
        :headers="headers"
        :items="userInfoModels"
        class="elevation-2"
        :server-items-length="pagedList.rowCount"
        :items-per-page.sync="paginationModel.pageSize"
        :page.sync="paginationModel.page"
      >
        <template #item.actions="{ item }">
          <v-icon md class="mr-2" @click="showuserInfoModels(item.productInfoModels)">mdi-menu-open</v-icon>
        </template>
      </v-data-table>
     </v-row>

      <v-dialog v-model="dialogEnable" :max-width="dialogWidth" :key="dialogKey">
        <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog"></component>
      </v-dialog>
  </v-container>
</template>

<script>
import UsageDetails from "./UsageDetails";
export default {
  data() {
    return {
      usage: [],
      headers: [
        { text: this.translator("company"), value: "company" },
        { text: this.translator("fullName"), value: "fullname" },
        { text: this.translator("totalUsageMinute"), value: "totalUsageMinute" },
        { text: this.translator("firstUsageDate"), value: "firstUsageDate" },
        { text: this.translator("lastUsageDate"), value: "lastUsageDate", sortable: false },
        { text: this.translator("actions"), value: "actions" },
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
        pageSize: 50,
      },
    };
  },
  created() {
    var root = "https://apicore.ait.com.tr/api/Files/Images/usage.json";
    this.$http.get(root).then((resp) => {
      if (resp.status == 200) {
        this.usage = resp.data;
        resp.data.userInfoModels.forEach((element) => {
          this.userInfoModels.push(element);
        });
      }
    });
  },
  methods: {
    showuserInfoModels(productInfoModels) {
      this.selectedComponent = UsageDetails;
      this.componentProps = { productInfoModelsProp: productInfoModels };

      this.openDialog(700);
    },
    openDialog(width = 700) {
      this.dialogEnable = true;
      this.dialogWidth = width;
    },
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

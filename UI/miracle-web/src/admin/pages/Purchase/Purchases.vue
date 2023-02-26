<template>
  <v-layout child-flex class="ml-5 mr-5">
    <v-data-table
      :headers="headers"
      :items="purchases"
      sort-by="id"
      class="elevation-5"
      :server-items-length="pagedList.rowCount"
      :items-per-page.sync="paginationModel.pageSize"
      :page.sync="paginationModel.page"
      :loading="loading"
    >
      <template #top>
        <v-toolbar flat color="white">
          <v-toolbar-title>{{ translator("purchases") }}</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer></v-spacer>
        </v-toolbar>
      </template>

      <template #item.actions="{ item }">
        <v-icon md class="mr-2" @click="showDeleteConfirm(item)">mdi-delete</v-icon>
      </template>
    </v-data-table>
    <v-dialog v-model="dialogEnable" :max-width="dialogWidth" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog"></component>
    </v-dialog>
  </v-layout>
</template>

<script>
import MessageBox from "@/helpers/components/MessageBox";
import PurchaseService from "@/services/PurchaseService";
import UserService from "@/services/UserService";
import VersionInfoService from "@/services/VersionInfoService";

export default {
  data() {
    return {
      purchaseService: new PurchaseService(),
      userService: new UserService(),
      versionService: new VersionInfoService(),
      headers: [
        { text: this.translator("username"), value: "username" },
        { text: this.translator("tag"), value: "tag" },
        { text: this.translator("version"), value: "version" },
        { text: this.translator("price"), value: "price" },
        { text: this.translator("quantity"), value: "quantity" },
        { text: this.translator("totalPrice"), value: "totalPrice" },
        { text: this.translator("currency"), value: "currency" },
        { text: this.translator("currencyValue"), value: "currencyValue" },
        { text: this.translator("actions"), value: "actions", sortable: false },
      ],
      purchases: [],
      selectedPurchase: {},
      pagedList: {},
      paginationModel: {
        page: 1,
        pageSize: 5,
      },
      dialogEnable: false,
      dialogKey: 0,
      dialogWidth: null,
      selectedComponent: "",
      componentProps: null,

      loading: true,
    };
  },
  created() {
    this.initialize();
  },
  methods: {
    initialize() {
      this.loading = true;
      this.purchases = [];
      this.purchaseService
        .getList(this.paginationModel)
        .then((response) => {
          if (response === 999) this.initialize();
          this.pagedList = response.pagedList;
          response.pagedList.list.forEach((purchase) => {
            this.userService.getById(purchase.userId).then((userResp) => {
              this.versionService.getById(purchase.versionId).then((versionResp) => {
                purchase.username = userResp.data.username;
                purchase.version = versionResp.data;
                this.purchases.push(purchase);
              });
            });
          });
        })
        .finally(() => {
          this.loading = false;
        });
    },
    showDeleteConfirm(purchase) {
      this.selectedPurchase = purchase;
      this.selectedComponent = MessageBox;
      this.componentProps = {
        messageTitleProp: this.translator("deleteTitle"),
        messageTextProp: this.translator("deleteMessage"),
        cancelButtonVisibleProp: true,
      };

      this.openDialog();
    },
    deletePurchase(purchase) {
      const index = this.purchases.indexOf(purchase);
      this.purchaseService.delete(purchase.id).then((response) => {
        if (response.success) this.purchases.splice(index, 1);
        else {
          this.selectedComponent = MessageBox;
          this.componentProps = {
            messageTitleProp: this.translator("failed"),
            messageTextProp: response.message,
            cancelButtonVisibleProp: false,
          };
        }
      });
    },
    openDialog(width = 400) {
      this.dialogEnable = true;
      this.dialogWidth = width;
    },
    closeDialog(signalModel) {
      const changes = signalModel.changes;
      const returnValues = signalModel.returnValues;
      if (changes) {
        this.setReturnValues(returnValues);
        this.initialize();
      }
      if (this.selectedComponent === MessageBox) {
        if (changes) this.deletePurchase(this.selectedPurchase);
      }

      this.dialogEnable = false;
      this.dialogKey += 1;
    },
    setReturnValues(values) {
      return values;
    },
  },
  watch: {
    "paginationModel.page"() {
      this.initialize();
    },
    "paginationModel.pageSize"() {
      this.initialize();
    },
  },
};
</script>

<style scoped></style>

<template>
  <v-layout child-flex class="ml-5 mr-5">
    <v-data-table
      :headers="headers"
      :items="users"
      item-key="id"
      sort-by="Id"
      class="elevation-5 mx-auto"
      :server-items-length="pagedList.rowCount"
      :items-per-page.sync="paginationModel.pageSize"
      :page.sync="paginationModel.page"
      :search.sync="paginationModel.searchFilter"
      :loading="loading"
    >
      <template #top>
        <v-toolbar flat color="white">
          <v-toolbar-title style="min-width: 60px;">{{ translator("users") }}</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>

          <v-spacer></v-spacer>
          <v-btn color="primary" dark class="mb-2" @click="showFilter">{{ translator("filter") }}</v-btn>

          <v-divider class="mx-4" inset vertical></v-divider>
          <v-btn color="primary" dark class="mb-2" @click="showCreate">{{ translator("create") }}</v-btn>
        </v-toolbar>
      </template>

      <template #item.isActive="{ item }">
        <v-icon>{{ item.isActive ? "mdi-check-bold" : "mdi-close-circle" }}</v-icon>
      </template>

      <template #item.actions="{ item }">
        <v-icon md class="mr-2" @click="showProducts(item)">mdi-apps-box</v-icon>
        <v-icon md class="mr-2" @click="showEdit(item)">mdi-pencil</v-icon>
        <v-icon md @click="showDeleteConfirm(item)">mdi-delete</v-icon>
      </template>
    </v-data-table>

    <v-dialog v-model="dialogEnable" :max-width="dialogWidth" :key="dialogKey" persistent ref="dialog">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog"></component>
    </v-dialog>
  </v-layout>
</template>

<script>
import MessageBox from "@/helpers/Components/MessageBox";
import DataTableFilter from "./components/DataTableFilter";
import UserCreate from "./components/UserCreate";
import UserEdit from "./components/UserEdit";
import Products from "./components/Products";
import UserService from "@/services/UserService";
import { ReturnConstraints } from "@/helpers/Constraints";

export default {
  data() {
    return {
      headers: [
        { text: this.translator("name"), value: "name" },
        { text: this.translator("surname"), value: "surname" },
        { text: this.translator("username"), value: "username" },
        { text: this.translator("email"), value: "email" },
        { text: this.translator("isActive"), value: "isActive" },
        { text: this.translator("actions"), value: "actions", sortable: false },
      ],
      users: [],
      selectedUser: {},
      userId: 0,
      editedUser: null,
      pagedList: {},
      paginationModel: {
        page: 1,
        pageSize: 5,
        propertyName: "",
        filterType: "",
        searchValue: "",
      },
      selectedComponent: "",
      componentProps: null,
      dialogEnable: false,
      dialogWidth: null,
      dialogKey: 1,

      loading: true,
    };
  },
  methods: {
    async initialize() {
      this.loading = true;
      let response = await new UserService().getList(this.paginationModel).finally(() => {
        this.loading = false;
      });
      if (response === ReturnConstraints.REFRESH) await this.initialize();
      this.users = response.pagedList.list;
      this.pagedList = response.pagedList;
    },
    showCreate() {
      this.selectedComponent = UserCreate;
      this.componentProps = null;

      this.openDialog(550);
    },
    showEdit(user) {
      this.selectedComponent = UserEdit;
      this.componentProps = { userIdProp: user.id };

      this.openDialog(550);
    },
    showFilter() {
      this.selectedComponent = DataTableFilter;
      this.componentProps = {
        propertyNameProp: this.paginationModel.propertyName,
        filterTypeProp: this.paginationModel.filterType,
        searchValueProp: this.paginationModel.searchValue,
      };
      this.openDialog();
    },
    showProducts(user) {
      this.selectedComponent = Products;
      this.componentProps = { userIdProp: user.id, usernameProp: user.username };

      this.openDialog(1000);
    },
    showDeleteConfirm(user) {
      this.selectedUser = user;
      this.selectedComponent = MessageBox;
      this.componentProps = {
        messageTitleProp: this.translator("deleteTitle"),
        messageTextProp: this.translator("deleteMessage"),
        cancelButtonVisibleProp: true,
      };

      this.openDialog(400);
    },
    async deleteUser(user) {
      const index = this.users.indexOf(user);
      let response = await new UserService().delete(user.id);
      if (response.success) this.users.splice(index, 1);
      else {
        this.selectedComponent = MessageBox;
        this.componentProps = {
          messageTitleProp: this.translator("failed"),
          messageTextProp: response.message,
          cancelButtonVisibleProp: false,
        };
      }
    },
    openDialog(width = 500) {
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
        if (changes) this.deleteUser(this.selectedUser);
      }

      this.dialogEnable = false;
      this.dialogKey += 1;
    },
    setReturnValues(values) {
      if (this.selectedComponent === DataTableFilter) {
        this.paginationModel = { ...values };
      }
    },
  },
  created() {
    this.initialize();
  },
  watch: {
    "paginationModel.page"() {
      this.initialize();
    },
    "paginationModel.pageSize"() {
      this.initialize();
    },
    "paginationModel.searchFilter"() {
      this.initialize();
    },
  },
};
</script>

<style scoped></style>

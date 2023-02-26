<template>
  <v-layout child-flex class="ml-5 mr-5">
    <v-data-table
      :headers="headers"
      :items="dependencies"
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
          <v-toolbar-title style="min-width: 60px;">{{ translator("dependencies") }}</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>

          <v-spacer></v-spacer>
          <v-btn color="red" dark class="mb-2" @click="stopServer"
            ><v-icon class="mr-1">mdi-location-exit</v-icon>{{ translator("stopServer") }}</v-btn
          >

          <v-divider class="mx-4" inset vertical></v-divider>
          <v-btn color="primary" dark class="mb-2" @click="showCreate">{{ translator("create") }}</v-btn>
        </v-toolbar>
      </template>

      <template #item.publishDate="{ item }">
        {{ new Date(item.publishDate).toISOString().substr(0, 19).replace("T", " ") }}
      </template>

      <template #item.lastChangeDate="{ item }">
        {{ new Date(item.lastChangeDate).toISOString().substr(0, 19).replace("T", " ") }}
      </template>

      <template #item.isLoaded="{ item }">
        <v-icon>{{ item.isLoaded ? "mdi-check-bold" : "mdi-close-circle" }}</v-icon>
      </template>

      <template #item.isActive="{ item }">
        <v-icon>{{ item.isActive ? "mdi-check-bold" : "mdi-close-circle" }}</v-icon>
      </template>

      <template #item.actions="{ item }">
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
import DependencyService from "@/services/DependencyService";
import MessageBox from "@/helpers/Components/MessageBox";
import DependencyCreate from "./components/DependencyCreate";
import DependencyEdit from "./components/DependencyEdit";
import { ReturnConstraints } from "@/helpers/Constraints";

export default {
  data() {
    return {
      headers: [
        { text: this.translator("libName"), value: "libName" },
        { text: this.translator("publishDate"), value: "publishDate" },
        { text: this.translator("lastChangeDate"), value: "lastChangeDate" },
        { text: this.translator("isLoaded"), value: "isLoaded" },
        { text: this.translator("isActive"), value: "isActive" },
        { text: this.translator("actions"), value: "actions", sortable: false },
      ],
      dependencies: [],
      selectedDependency: {},
      dependencyId: 0,
      editedDependency: null,
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
      let response = await new DependencyService().getList(this.paginationModel).finally(() => {
        this.loading = false;
      });
      if (response === ReturnConstraints.REFRESH) await this.initialize();
      this.dependencies = response.pagedList.list;
      this.pagedList = response.pagedList;
    },
    showCreate() {
      this.selectedComponent = DependencyCreate;
      this.componentProps = null;

      this.openDialog(550);
    },
    showEdit(dependency) {
      this.selectedComponent = DependencyEdit;
      this.componentProps = { dependencyIdProp: dependency.id };

      this.openDialog(550);
    },
    showDeleteConfirm(dependency) {
      this.selectedDependency = dependency;
      this.selectedComponent = MessageBox;
      this.componentProps = {
        messageTitleProp: this.translator("deleteTitle"),
        messageTextProp: this.translator("deleteMessage"),
        cancelButtonVisibleProp: true,
      };

      this.openDialog(400);
    },
    async deleteDependency(dependency) {
      const index = this.dependencies.indexOf(dependency);
      let response = await new DependencyService().delete(dependency.id);
      if (response.success === true) this.dependencies.splice(index, 1);
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
        if (changes) this.deleteDependency(this.selectedDependency);
      }

      this.dialogEnable = false;
      this.dialogKey += 1;
    },
    setReturnValues(values) {
      return values;
    },
    async stopServer() {
      if (confirm(this.translator("stopServerConfirmMessage"))) {
        let response = await new DependencyService().stopServer();
        if (!response.success) {
          this.selectedComponent = MessageBox;
          this.componentProps = {
            messageTitleProp: this.translator("failed"),
            messageTextProp: response.message,
            cancelButtonVisibleProp: false,
          };
        }
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

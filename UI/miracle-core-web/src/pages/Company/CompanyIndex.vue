<template>
  <v-layout child-flex class="ml-5 mr-5">
    <v-data-table
      :headers="headers"
      :items="companies"
      sort-by="Name"
      class="elevation-5"
      :server-items-length="pagedList.rowCount"
      :items-per-page.sync="paginationModel.pageSize"
      :page.sync="paginationModel.page"
      :loading="loading"
    >
      <template #top>
        <v-toolbar flat color="white">
          <v-toolbar-title>{{ translator("companies") }}</v-toolbar-title>

          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer></v-spacer>

          <v-btn @click="showCreate" color="primary" dark class="mb-2">{{ translator("create") }}</v-btn>
        </v-toolbar>
      </template>

      <template #item.location="{ item }">
        <div :style="{ maxWidth: '250px', overflowX: 'scroll' }">{{ item.location }}</div>
      </template>

      <template #item.actions="{ item }">
        <v-icon md class="mr-2" @click="showEdit(item)">mdi-pencil</v-icon>
        <v-icon md @click="showDeleteConfirm(item)">mdi-delete</v-icon>
      </template>
    </v-data-table>
    <v-dialog v-model="dialogEnable" max-width="400" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog" />
    </v-dialog>
  </v-layout>
</template>

<script>
import MessageBox from "@/helpers/Components/MessageBox";
import CompanyCreate from "./components/CompanyCreate";
import CompanyEdit from "./components/CompanyEdit";
import CompanyService from "@/services/CompanyService";
import { ReturnConstraints } from "@/helpers/Constraints";

export default {
  data() {
    return {
      headers: [
        { text: this.translator("name"), value: "name" },
        { text: this.translator("address"), value: "address" },
        { text: this.translator("location"), value: "location" },
        { text: this.translator("phone"), value: "phoneNumber" },
        { text: this.translator("actions"), value: "actions", sortable: false },
      ],
      companies: [],
      selectedCompany: {},
      pagedList: {},
      paginationModel: {
        page: 1,
        pageSize: 5,
      },
      dialogEnable: false,
      dialogKey: 0,
      selectedComponent: "",
      componentProps: null,

      loading: true,
    };
  },
  methods: {
    async initialize() {
      this.loading = true;
      let response = await new CompanyService().getList(this.paginationModel).finally(() => {
        this.loading = false;
      });
      if (response === ReturnConstraints.REFRESH) await this.initialize();
      this.companies = response.pagedList.list;
      this.pagedList = response.pagedList;
    },
    showCreate() {
      this.selectedComponent = CompanyCreate;
      this.componentProps = null;

      this.dialogEnable = true;
    },
    showEdit(company) {
      this.selectedComponent = CompanyEdit;
      this.componentProps = {
        companyIdProp: company.id,
      };

      this.dialogEnable = true;
    },
    showDeleteConfirm(company) {
      this.selectedCompany = company;
      this.selectedComponent = MessageBox;
      this.componentProps = {
        messageTitleProp: this.translator("deleteTitle"),
        messageTextProp: this.translator("deleteMessage"),
        cancelButtonVisibleProp: true,
      };

      this.dialogEnable = true;
    },
    async deleteCompany(company) {
      const index = this.companies.indexOf(company);

      let response = await new CompanyService().delete(company.id);
      if (response.success) this.companies.splice(index, 1);
      else {
        this.selectedComponent = MessageBox;
        this.componentProps = {
          messageTitleProp: this.translator("failed"),
          messageTextProp: response.message,
          cancelButtonVisibleProp: false,
        };
      }
    },
    closeDialog(signalModel) {
      const changes = signalModel.changes;
      const returnValues = signalModel.returnValues;
      if (changes) {
        this.setReturnValues(returnValues);
        this.initialize();
      }
      if (this.selectedComponent === MessageBox) {
        if (changes) this.deleteCompany(this.selectedCompany);
      }

      this.dialogEnable = false;
      this.dialogKey += 1;
    },
    setReturnValues(values) {
      return values;
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
  },
};
</script>

<style scoped></style>

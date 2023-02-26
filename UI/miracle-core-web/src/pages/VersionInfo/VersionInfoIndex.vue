<template>
  <v-layout child-flex class="ml-5 mr-5">
    <v-data-table
      :headers="headers"
      :items="orderedVersionInfos"
      class="elevation-5"
      :server-items-length="pagedList.rowCount"
      :items-per-page.sync="paginationModel.pageSize"
      :page.sync="paginationModel.page"
      :loading="loading"
    >
      <template #top>
        <v-toolbar flat color="white">
          <v-toolbar-title>{{ translator("versionInfo") }}</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer></v-spacer>

          <v-btn @click="showCreate" color="primary" dark class="mb-2">
            {{ translator("create") }}
          </v-btn>
        </v-toolbar>
      </template>

      <template #item.actions="{ item }">
        <v-icon md class="mr-2" @click="showEdit(item)">mdi-pencil</v-icon>
        <v-icon md @click="showDeleteConfirm(item)">mdi-delete</v-icon>
      </template>
    </v-data-table>

    <v-dialog v-model="dialogEnable" :max-width="dialogWidth" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog"></component>
    </v-dialog>
  </v-layout>
</template>

<script>
import VersionInfoCreate from "./components/VersionInfoCreate";
import MessageBox from "@/helpers/Components/MessageBox";
import VersionInfoEdit from "./components/VersionInfoEdit";
import VersionInfoService from "@/services/VersionInfoService";
import { ReturnConstraints } from "@/helpers/Constraints";
import _ from "lodash";

export default {
  data() {
    return {
      headers: [
        { text: this.translator("version"), value: "version" },
        { text: this.translator("actions"), value: "actions", sortable: false },
      ],
      versionInfos: [],
      selectedVersionInfo: {},
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
    async initialize() {
      this.loading = true;
      let response = await new VersionInfoService().getList(this.paginationModel).finally(() => {
        this.loading = false;
      });
      if (response === ReturnConstraints.REFRESH) await this.initialize();
      this.versionInfos = response.pagedList.list;
      this.pagedList = response.pagedList;
    },
    showCreate() {
      this.selectedComponent = VersionInfoCreate;
      this.componentProps = null;

      this.openDialog(400);
    },
    showEdit(versionInfo) {
      this.selectedComponent = VersionInfoEdit;
      this.componentProps = { versionInfoIdProp: versionInfo.id };

      this.openDialog(400);
    },
    showDeleteConfirm(versionInfo) {
      this.selectedVersionInfo = versionInfo;
      this.selectedComponent = MessageBox;
      this.componentProps = {
        messageTitleProp: this.translator("deleteTitle"),
        messageTextProp: this.translator("deleteMessage"),
        cancelButtonVisibleProp: true,
      };

      this.openDialog(400);
    },
    async deleteVersionInfo(versionInfo) {
      const index = this.versionInfos.indexOf(versionInfo);
      let response = await new VersionInfoService().delete(versionInfo.id);
      if (response.success) this.versionInfos.splice(index, 1);
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
        if (changes) this.deleteVersionInfo(this.selectedVersionInfo);
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
  computed: {
    orderedVersionInfos() {
      return _.sortBy(this.versionInfos, "version");
    },
  },
};
</script>

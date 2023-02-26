<template>
  <v-card>
    <v-data-table
      :headers="headers"
      :items="orderedVersionInfos"
      :server-items-length="pagedList.rowCount"
      :items-per-page.sync="paginationModel.pageSize"
      :page.sync="paginationModel.page"
      :loading="loading"
    >
      <template #top>
        <v-toolbar flat color="dark">
          <v-toolbar-title>{{ translator("versionInfos") }}</v-toolbar-title>

          <v-list class="ma-5">
            ( <span class="ma-1" v-for="platform in platforms" :key="platform.id">{{ platform.name }}</span> )
          </v-list>

          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer></v-spacer>
          <v-btn class="mb-2 ml-1 mr-1" color="primary" dark @click="showAddVersionInfo">
            {{ translator("add") }}
          </v-btn>
          <v-btn class="mb-2" color="red" dark @click="close(false)">{{ translator("close") }}</v-btn>
        </v-toolbar>
      </template>

      <template #item.actions="{ item }">
        <v-icon md class="mr-2" @click="showAddSetup(item)">mdi-folder</v-icon>
        <v-icon md @click="showRemoveConfirm(item)">mdi-delete</v-icon>
      </template>
    </v-data-table>

    <v-dialog v-model="dialogEnable" :max-width="dialogWidth" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog"></component>
    </v-dialog>
  </v-card>
</template>

<script>
import MessageBox from "@/helpers/Components/MessageBox";
import ProductAddSetup from "./ProductAddSetup";
import ProductVersionSetting from "./ProductVersionSetting";
import PlatformService from "@/services/PlatformService";
import PriorityService from "@/services/PriorityService";
import ProductService from "@/services/ProductService";
import VersionInfoService from "@/services/VersionInfoService";
import { ReturnConstraints } from "@/helpers/Constraints";
import _ from "lodash";

export default {
  props: ["productIdProp"],
  data() {
    return {
      headers: [
        { text: this.translator("version"), value: "version" },
        { text: this.translator("priority"), value: "priority" },
        { text: this.translator("actions"), value: "actions", sortable: false },
      ],
      versionInfos: [],
      selectedVersionInfo: {},
      selectedVersionInfoId: 0,
      platforms: [],
      productId: this.productIdProp,
      paginationModel: {
        page: 1,
        pageSize: 5,
      },
      pagedList: {},
      signalModel: {
        changes: false,
        returnValues: null,
      },

      dialogEnable: false,
      dialogKey: 0,
      dialogWidth: 400,
      selectedComponent: "",
      componentProps: {},

      loading: true,
    };
  },
  methods: {
    async initialize() {
      this.loading = true;

      this.versionInfos = [];
      let versionResponse = await new VersionInfoService().getVersionListByProductId(this.productId, this.paginationModel)
        .finally(() => {
          this.loading = false;
        });
      let platformResponse = await new PlatformService().getListByProductId(this.productId);

      if ((versionResponse || platformResponse) === ReturnConstraints.REFRESH) await this.initialize();

      this.platforms = platformResponse.list;
      this.pagedList = versionResponse.pagedList;
      const versionInfos = versionResponse.pagedList.list;

      for (const versionInfo of versionInfos) {
        let priorityResponse = await new PriorityService().getPriorityByVersion(this.productId, versionInfo.id);
        if (priorityResponse === ReturnConstraints.REFRESH) await this.initialize();

        const priority = priorityResponse.data;
        let priorityValue = priority.state + " - " + priority.name;
        this.versionInfos.push({
          id: versionInfo.id,
          version: versionInfo.version,
          priority: priorityValue,
        });
      }
    },
    showAddSetup(versionInfo) {
      this.selectedComponent = ProductAddSetup;
      this.componentProps = {
        versionInfoIdProp: versionInfo.id,
        productIdProp: this.productIdProp,
      };

      this.openDialog();
    },
    showAddVersionInfo() {
      this.selectedComponent = ProductVersionSetting;
      this.componentProps = {
        productIdProp: this.productIdProp,
      };

      this.openDialog();
    },
    showRemoveConfirm(versionInfo) {
      this.selectedVersionInfo = versionInfo;
      this.selectedComponent = MessageBox;
      this.componentProps = {
        messageTitleProp: this.translator("deleteTitle"),
        messageTextProp: this.translator("deleteMessage"),
        cancelButtonVisibleProp: true,
      };

      this.dialogEnable = true;
    },
    async removeVersion(versionInfo) {
      const index = this.versionInfos.indexOf(versionInfo);
      let response = await new ProductService().productRemoveVersion(this.productId, versionInfo.id);
      if (response.success) {
        this.versionInfos.forEach((item) => {
          if (item.id === versionInfo.id) 
            this.versionInfos.splice(index, 1);
        });
      } else {
        this.selectedComponent = MessageBox;
        this.componentProps = {
          messageTitleProp: this.translator("failed"),
          messageTextProp: response.message,
          cancelButtonVisibleProp: false,
        };
      }
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
        if (changes) this.removeVersion(this.selectedVersionInfo);
      }

      this.dialogEnable = false;
      this.dialogKey += 1;
    },
    setReturnValues(values) {
      return values;
    },
    close(changes) {
      this.signalModel.changes = changes;
      this.$emit("closed", this.signalModel);
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
  computed: {
    orderedVersionInfos() {
      return _.sortBy(this.versionInfos, "version");
    },
  },
};
</script>

<style scoped></style>

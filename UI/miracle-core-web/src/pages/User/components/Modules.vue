<template>
  <v-card>
    <v-layout child-flex>
      <v-data-table
        :headers="headers"
        :items="modules"
        item-key="id"
        sort-by="Id"
        class="elevation-5 mx-auto"
        :server-items-length="pagedList.rowCount"
        :items-per-page.sync="paginationModel.pageSize"
        :page.sync="paginationModel.page"
        :loading="loading"
      >
        <template #top>
          <v-toolbar flat color="white">
            <v-toolbar-title style="min-width: 60px;">
              {{ username + "'s " + translator("productModules") }}
            </v-toolbar-title>

            <v-divider class="mx-4" inset vertical></v-divider>
            <v-spacer></v-spacer>

            <v-btn color="primary" dark class="mb-2" @click="showAddModule">
              {{ translator("add") }}
            </v-btn>

            <div class="ml-1 mr-1"></div>

            <v-btn color="red" dark class="mb-2" @click="close(false)">{{ translator("close") }}</v-btn>
          </v-toolbar>
        </template>

        <template #item.description="{ item }">
          <div class="description">
            {{ item.description }}
          </div>
        </template>

        <template #item.isActive="{ item }">
          <v-switch v-model="item.isActive" @change="changeActive(item)" />
        </template>

        <template #item.actions="{ item }">
          <v-icon md class="mr-2" @click="showDeleteConfirm(item)">mdi-delete</v-icon>
        </template>
      </v-data-table>
      <v-dialog v-model="dialogEnable" :max-width="dialogWidth" persistent :key="dialogKey">
        <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog"></component>
      </v-dialog>
    </v-layout>
  </v-card>
</template>

<script>
import MessageBox from "@/helpers/Components/MessageBox";
import ModuleAdd from "./ModuleAdd";
import UserService from "@/services/UserService";
import { ReturnConstraints } from "@/helpers/Constraints";

export default {
  props: ["userIdProp", "usernameProp", "productIdProp"],
  data() {
    return {
      headers: [
        { text: this.translator("name"), value: "name" },
        { text: this.translator("description"), value: "description" },
        { text: this.translator("isActive"), value: "isActive" },
        { text: this.translator("actions"), value: "actions", sortable: false },
      ],

      username: "",
      userId: null,
      productId: null,
      moduleId: null,
      selectedProductModule: {},
      modules: [],
      pagedList: {},
      paginationModel: {
        page: 1,
        pageSize: 5,
        searchFilter: "",
        propertyName: "",
        filterType: "",
      },
      signalModel: {
        changes: false,
        returnValues: null,
      },

      dialogEnable: false,
      dialogKey: 1,
      dialogWidth: 400,
      selectedComponent: "",
      componentProps: {},

      loading: true,

      snack: false,
      snackColor: "",
      snackText: "",
      pagination: {},
    };
  },
  created() {
    this.userId = this.userIdProp;
    this.productId = this.productIdProp;
    this.username = this.usernameProp;
    this.initialize();
  },
  methods: {
    async initialize() {
      this.loading = true;
      this.modules = [];

      let response = await new UserService()
        .getProductModules(this.userId, this.productId, this.paginationModel)
        .finally(() => {
          this.loading = false;
        });
      if (response === ReturnConstraints.REFRESH) await this.initialize();
      response.pagedList.list.forEach((item) => {
        item.module.isActive = item.isActive;
        this.modules.push(item.module);
      });
    },
    showAddModule() {
      this.selectedComponent = ModuleAdd;
      this.componentProps = { userIdProp: this.userId, productIdProp: this.productId };

      this.openDialog();
    },
    async changeActive(module) {
      let response = await new UserService().userProductModuleAdd(
        this.userId,
        this.productId,
        module.id,
        module.isActive
      );
      if (!response.success) {
        this.selectedComponent = MessageBox;
        this.componentProps = {
          messageTitleProp: this.translator("failed"),
          messageTextProp: response.message,
          cancelButtonVisibleProp: false,
        };
      }
    },
    showDeleteConfirm(userProductModule) {
      this.selectedProductModule = userProductModule;
      this.selectedComponent = MessageBox;
      this.componentProps = {
        messageTitleProp: this.translator("deleteTitle"),
        messageTextProp: this.translator("deleteMessage"),
        cancelButtonVisibleProp: true,
      };

      this.openDialog();
    },
    async deleteProductModule(module) {
      let response = await new UserService().removeUserProductModule(this.userId, this.productId, module.id);
      if (response.success) {
        this.modules.forEach((item) => {
          if (item.id === module.id) this.modules.splice(this.modules.indexOf(module), 1);
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

      if (this.selectedComponent === MessageBox) {
        if (changes) this.deleteProductModule(this.selectedProductModule);
      }
      if (changes) {
        this.setReturnValues(returnValues);
        this.initialize();
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

<style scoped>
.description {
  max-width: 400px;
  max-height: 60px;
  overflow-y: scroll;
  padding: 15px 15px;
}
</style>

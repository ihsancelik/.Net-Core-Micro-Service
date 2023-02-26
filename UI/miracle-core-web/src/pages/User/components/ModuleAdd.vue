<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)">
        <v-icon style="float: right;">mdi-close-circle</v-icon>
      </a>
      <h2 class="text-md-center pt-0">
        {{ translator("userAddProductModule") }}
      </h2>
      <hr class="mb-3" />

      <v-select
        v-model="model.moduleId"
        :items="productModules"
        item-value="id"
        item-text="name"
        :placeholder="translator('selectModule')"
        outlined
        small-chips
        required
      ></v-select>

      <v-switch v-model="model.isActive" :label="translator('isActive')" required outlined></v-switch>

      <hr class="mb-2" />

      <v-btn @click.prevent="productModuleAdd" color="primary" width="100%" class="v-btn--block v-size--large mb-2">
        {{ productIdProp > 0 ? translator("update") : translator("add") }}
      </v-btn>
    </v-card>

    <v-dialog v-model="dialogEnable" max-width="400" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog" />
    </v-dialog>
  </ValidationObserver>
</template>

<script>
import * as rules from "vee-validate/dist/rules";
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import MessageBox from "@/helpers/Components/MessageBox";
import UserService from "@/services/UserService";
import ProductModuleService from "@/services/ProductModuleService";
import { ReturnConstraints } from "@/helpers/Constraints";

setInteractionMode("eager");

extend("required", { ...rules.required, message: "{_field_} can not be empty" });

export default {
  props: ["userIdProp", "productIdProp"],
  data() {
    return {
      productModules: [],
      model: {
        userId: 0,
        productId: 0,
        moduleId: 0,
        isActive: false,
      },

      signalModel: {
        changes: false,
        returnValues: null,
      },
      dialogEnable: false,
      dialogKey: 0,
      selectedComponent: "",
      componentProps: null,
    };
  },
  created() {
    this.model.userId = this.userIdProp;
    this.model.productId = this.productIdProp;
    this.initialize();
  },
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  methods: {
    async initialize() {
      let response = await new ProductModuleService().getListAll();
      if (response === ReturnConstraints.REFRESH) await this.initialize();
      this.productModules = response.list;
    },
    async productModuleAdd() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let response = await new UserService().userProductModuleAdd(
          this.model.userId,
          this.model.productId,
          this.model.moduleId,
          this.model.isActive
        );
        if (response.success) {
          this.close("closed", true);
        } else {
          this.selectedComponent = MessageBox;
          this.componentProps = {
            messageTitleProp: this.translator("failed"),
            messageTextProp: response.message,
          };
          this.dialogEnable = true;
          this.dialogKey += 1;
        }
      }
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

<style></style>

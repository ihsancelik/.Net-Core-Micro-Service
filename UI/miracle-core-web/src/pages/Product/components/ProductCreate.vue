<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)">
        <v-icon style="float: right;">mdi-close-circle</v-icon>
      </a>
      <h2 class="text-md-center">{{ translator("newProduct") }}</h2>
      <hr class="mb-5" />

      <v-form>
        <ValidationProvider #default="{ errors }" :name="translator('name')" rules="required|max:164">
          <v-text-field
            v-model="productModel.product.name"
            :label="translator('name')"
            :error-messages="errors"
            required
            outlined
            counter="164"
          ></v-text-field>
        </ValidationProvider>

        <ValidationProvider #default="{ errors }" :name="translator('description')" rules="required|max:512">
          <v-textarea
            v-model="productModel.product.description"
            :label="translator('description')"
            :error-messages="errors"
            required
            outlined
            counter="512"
          ></v-textarea>
        </ValidationProvider>

        <v-select
          v-model="productModel.product.tag"
          :items="productTags"
          :placeholder="translator('selectProductTag')"
          item-text="tag"
          item-value="tag"
          outlined
        ></v-select>

        <v-select
          multiple
          v-model="productModel.platformIdList"
          :items="platforms"
          item-value="id"
          item-text="name"
          :placeholder="translator('selectPlatform')"
          outlined
        ></v-select>

        <v-switch v-model="productModel.product.isPlugin" :label="translator('isPlugin')" required outlined></v-switch>

        <v-switch v-model="productModel.product.isActive" :label="translator('isActive')" required outlined></v-switch>

        <hr class="mb-2" />

        <v-btn @click.prevent="productCreate" color="primary" dark class="v-btn--block v-size--large mb-2">
          {{ translator("create") }}
        </v-btn>
      </v-form>
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
import PlatformService from "@/services/PlatformService";
import ProductService from "@/services/ProductService";
import ProductTagService from "@/services/ProductTagService";
import { ReturnConstraints } from "@/helpers/Constraints";

setInteractionMode("eager");
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });
extend("required", { ...rules.required, message: "{_field_} can not be empty" });

export default {
  data() {
    return {
      productModel: {
        product: {},
        platformIdList: [],
      },
      signalModel: {
        changes: false,
        returnValues: undefined,
      },
      productTags: [],
      platforms: [],

      dialogEnable: false,
      dialogKey: 0,
      selectedComponent: "",
      componentProps: null,
    };
  },
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  methods: {
    async initialize() {
      let tagResponse = await new ProductTagService().getListAll();
      let platformResponse = await new PlatformService().getListAll();
      if ((tagResponse || platformResponse) === ReturnConstraints.REFRESH) await this.initialize();

      this.productTags = tagResponse.list;
      this.platforms = platformResponse.list;
    },
    async productCreate() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let PlatformIdList = this.productModel.platformIdList;
        let model = { ...this.productModel.product, PlatformIdList };

        let response = await new ProductService().create(model);
        if (response.success) this.close("closed", true);
        else {
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
  created() {
    this.productModel.product.isActive = false;
    this.productModel.product.isPlugin = false;
    this.initialize();
  },
};
</script>

<style scoped></style>

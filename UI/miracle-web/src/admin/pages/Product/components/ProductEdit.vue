<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)"><v-icon style="float: right;">mdi-close-circle</v-icon></a>
      <h2 class="text-md-center">{{ translator("editProduct") }}</h2>
      <hr class="mb-5" />

      <v-form>
        <v-row id="rows1">
          <v-col cols="12" md="12">
            <v-file-input
              prepend-inner-icon="mdi-camera"
              accept=".jpg, .png, .jpeg"
              :placeholder="translator('productImage')"
              @change="uploadImage"
            ></v-file-input>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="12" md="12" class="text-center">
            <img
              id="vimg"
              src="/miracle-logo.png"
              style="display: inline-block; background-size: contain; max-height: 200px; height: 200px;"
            />
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="8">
            <v-select
              class="field"
              v-model="selectedProductTag"
              :items="products"
              item-value="tag"
              item-text="name"
              :placeholder="translator('selectProduct')"
              outlined
              disabled
            ></v-select>
          </v-col>
          <v-col cols="4">
            <v-select
              class="field"
              v-model="selectedVersion"
              :items="productVersions"
              item-value="version"
              item-text="version"
              :placeholder="translator('selectVersion')"
              outlined
              disabled
            ></v-select>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="7">
            <ValidationProvider #default="{ errors }" :name="translator('price')" rules="required">
              <v-text-field
                class="field"
                v-model="model.product.price"
                :label="translator('price')"
                :error-messages="errors"
                required
                outlined
              ></v-text-field>
            </ValidationProvider>
          </v-col>
          <v-col md="5">
            <v-select
              class="field"
              v-model="model.product.currency"
              :items="['EUR', 'USD', 'TRY', 'GBP']"
              :placeholder="translator('selectCurrency')"
              outlined
            ></v-select>
          </v-col>
        </v-row>
        <ValidationProvider #default="{ errors }" :name="translator('order')" rules="required|numeric">
          <v-text-field
            class="field"
            v-model="model.product.order"
            :label="translator('order')"
            :error-messages="errors"
            required
            outlined
          ></v-text-field>
        </ValidationProvider>

        <ValidationProvider #default="{ errors }" :name="translator('description')" rules="required|max:512">
          <v-textarea
            class="field"
            v-model="model.product.description"
            :label="translator('description')"
            :error-messages="errors"
            required
            outlined
            counter="512"
          ></v-textarea>
        </ValidationProvider>

        <v-switch v-model="model.product.isActive" :label="translator('isActive')" required outlined></v-switch>

        <hr class="mb-2" />

        <v-btn @click="productEdit" color="primary" dark class="v-btn--block v-size--large mb-2">
          {{ translator("update") }}
        </v-btn>
      </v-form>
    </v-card>

    <v-dialog v-model="dialogEnable" max-width="400" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog" />
    </v-dialog>
  </ValidationObserver>
</template>

<script>
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import { Base } from "../../../../helpers/RouteConstraints";
import * as rules from "vee-validate/dist/rules";
import MessageBox from "../../../../helpers/components/MessageBox";
import ProductService from "../../../../services/ProductService";
import { ReturnConstraints } from "@/helpers/Constraints";

setInteractionMode("eager");
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });
extend("required", { ...rules.required, message: "{_field_} can not be empty" });

export default {
  props: ["productIdProp"],
  data() {
    return {
      products: [],
      productVersions: [],
      selectedProductTag: "",
      selectedVersion: "",
      model: {
        product: {},
        productId: 0,
      },
      signalModel: {
        changes: false,
        returnValues: undefined,
      },

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
      let productsResponse = await new ProductService().getProducts();
      let productResponse = await new ProductService().getById(this.model.productId);

      if ((productsResponse || productResponse) === ReturnConstraints.REFRESH) await this.initialize();

      this.products = productsResponse.list;

      this.model.product.id = this.model.productId;
      if (productResponse) {
        this.model.product = productResponse.data;
        this.selectedProductTag = productResponse.data.tag;

        let imageResponse = await new ProductService().getImage(this.model.productId);
        const img = document.getElementById("vimg");
        img.src = Base + imageResponse;
      }
    },
    async productEdit() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let formData = new FormData();

        for (let [key, value] of Object.entries(this.model.product)) {
          formData.append(`${key}`, `${value}`);
        }
        formData.append("productImage", this.model.product.productImage);

        let response = await new ProductService().edit(formData, this.model.productId);
        if (response.success) this.close(true);
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
    uploadImage(image) {
      this.model.product.productImage = image;
      let reader = new FileReader();
      reader.onload = function (e) {
        document.getElementById("vimg").setAttribute("src", e.target.result.toString());
      };
      reader.readAsDataURL(image);
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
  watch: {
    selectedProductTag(val) {
      this.productVersions = [];
      this.products.forEach((prod) => {
        if (prod.tag === val) {
          prod.productSettings.forEach((item) => {
            this.productVersions.push(item.versionInfo);
          });
        }
      });
    },
  },
  created() {
    this.model.productId = this.productIdProp;
    this.initialize();
  },
};
</script>

<style scoped>
.field {
  margin-bottom: -4%;
}
</style>

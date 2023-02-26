<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)"><v-icon style="float: right;">mdi-close-circle</v-icon></a>
      <h2 class="text-md-center">{{ translator("newProduct") }}</h2>
      <hr class="mb-5" />

      <v-form>
        <v-row id="rows1">
          <v-col cols="12" md="12">
            <v-file-input
              prepend-inner-icon="mdi-camera"
              accept="image/*"
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
              alt=""
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
            ></v-select>
          </v-col>
          <v-col cols="4">
            <v-select
              class="field"
              v-model="selectedVersionId"
              :items="productVersions"
              item-text="version"
              item-value="id"
              :placeholder="translator('selectVersion')"
              outlined
            ></v-select>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="7">
            <ValidationProvider #default="{ errors }" :name="translator('price')" rules="required">
              <v-text-field
                class="field"
                v-model="product.price"
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
              v-model="product.currency"
              :items="['EUR', 'USD', 'TRY', 'GBP']"
              :placeholder="translator('selectCurrency')"
              outlined
            ></v-select>
          </v-col>
        </v-row>
        <ValidationProvider #default="{ errors }" :name="translator('order')" rules="required|numeric">
          <v-text-field
            class="field"
            v-model="product.order"
            :label="translator('order')"
            :error-messages="errors"
            required
            outlined
          ></v-text-field>
        </ValidationProvider>
        <ValidationProvider #default="{ errors }" :name="translator('description')" rules="required|max:512">
          <v-textarea
            class="field"
            v-model="product.description"
            :label="translator('description')"
            :error-messages="errors"
            required
            outlined
            counter="512"
          ></v-textarea>
        </ValidationProvider>

        <v-switch v-model="product.isActive" :label="translator('isActive')" required outlined></v-switch>

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
/* eslint-disable */
import * as rules from "vee-validate/dist/rules";
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import MessageBox from "../../../../helpers/components/MessageBox";
import ProductService from "../../../../services/ProductService";

import VersionInfoService from "@/services/VersionInfoService";

setInteractionMode("eager");
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });
extend("required", { ...rules.required, message: "{_field_} can not be empty" });

export default {
  data() {
    return {
      products: [],
      productVersions: [],
      versionInfos: [],
      product: {},
      selectedProductTag: "",
      selectedVersionId: 0,
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
  watch: {
    selectedProductTag(val) {
      this.productVersions = [];
      this.products.forEach((prod) => {
        if (prod.tag === val) {
          prod.productSettings.forEach((item) => {
            this.versionInfos.forEach((version) => {
              if (version.id === item.versionInfoId) this.productVersions.push(version);
            });
          });
        }
      });
    },
  },
  async created() {
    this.product.isActive = false;
    try {
      await new ProductService()
        .getProducts()
        .then((productResponse) => {
          this.products = productResponse.list;
        })
        .catch((err) => {
          console.log(err);
        });

      await new VersionInfoService().getListAll().then((versionResponse) => {
        this.versionInfos = versionResponse.list;
      });
    } catch (error) {
      console.log(error);
    }
  },
  methods: {
    async productCreate() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let formData = new FormData();
        this.products.forEach((product) => {
          if (product.tag === this.selectedProductTag) {
            this.product.name = product.name;
            this.product.tag = product.tag;
            this.product.publishDate = product.publishDate;
            this.product.versionId = this.selectedVersionId;
          }
        });

        for (let [key, value] of Object.entries(this.product)) {
          formData.append(`${key}`, `${value}`);
        }
        formData.append("productImage", this.product.productImage);
        let response = await new ProductService().create(formData);
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
    uploadImage(image) {
      this.product.productImage = image;
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
};
</script>

<style scoped>
.field {
  margin-bottom: -4%;
}
</style>

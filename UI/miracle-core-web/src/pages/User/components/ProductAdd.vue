<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)">
        <v-icon style="float: right;">mdi-close-circle</v-icon>
      </a>
      <h2 class="text-md-center pt-0">
        {{ model.productId > 0 ? selectedProductName : translator("userAddProduct") }}
      </h2>
      <hr class="mb-3" />

      <v-form>
        <ValidationProvider #default="{ errors }" :name="translator('products')" rules="required">
          <v-select
            v-model="model.productId"
            :items="products"
            item-value="id"
            item-text="name"
            :placeholder="translator('selectProduct')"
            outlined
            small-chips
            required
            :error-messages="errors"
          ></v-select>
        </ValidationProvider>
        <ValidationProvider #default="{ errors }" :name="translator('versionInfos')" rules="required">
          <v-select
            multiple
            v-model="model.versionInfoIdList"
            :items="orderedVersionInfos"
            item-value="id"
            item-text="version"
            :placeholder="translator('selectVersion')"
            outlined
            small-chips
            required
            :error-messages="errors"
          ></v-select>
        </ValidationProvider>
        <v-switch
          v-model="model.productLimitation.isActive"
          :label="translator('isActive')"
          required
          outlined
        ></v-switch>

        <v-switch v-model="model.productLimitation.isDemo" :label="translator('isDemo')" required outlined></v-switch>

        <v-date-picker
          v-if="model.productLimitation.isDemo"
          v-model="dates"
          :min="new Date().toISOString()"
          range
          no-title
          width="100%"
          style="margin-bottom: 3%;"
        ></v-date-picker>

        <hr class="mb-2" />

        <v-btn @click.prevent="productAdd" color="primary" width="100%" class="v-btn--block v-size--large mb-2">
          {{ productIdProp > 0 ? translator("update") : translator("add") }}
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
import UserService from "@/services/UserService";
import ProductService from "@/services/ProductService";
import VersionInfoService from "@/services/VersionInfoService";
import { ReturnConstraints } from "@/helpers/Constraints";
import _ from "lodash";

setInteractionMode("eager");

extend("required", { ...rules.required, message: "{_field_} can not be empty" });

export default {
  props: ["userIdProp", "productIdProp"],
  data() {
    return {
      products: [],
      selectedProductName: "",
      versionInfos: [],
      dates: [],
      pagedList: {},
      model: {
        userId: null,
        productId: null,
        versionInfoIdList: [],
        productLimitation: {
          isActive: false,
          isDemo: false,
          demoStartDate: "",
          demoEndDate: "",
        },
      },
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
  watch: {
    "model.productId"(productId) {
      this.dates.push(new Date().toISOString().substr(0, 10));
      this.model.productLimitation.isActive = false;
      this.model.productLimitation.isDemo = false;

      this.initializeWithValue(productId);
    },
  },
  computed: {
    orderedVersionInfos() {
      return _.sortBy(this.versionInfos, ["version"]);
    },
  },
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  methods: {
    async initialize() {
      let response = await new ProductService().getListAll();
      if (response === ReturnConstraints.REFRESH) await this.initialize();

      this.products = response.list;
      this.products.forEach((product) => {
        if (product.id === this.productIdProp) this.selectedProductName = product.name;
      });
    },
    async initializeWithValue(productId) {
      if (productId > 0) {
        let versionResponse = await new VersionInfoService().getVersionListByUserProduct(productId);
        let limitResponse = await new UserService().getProductLimitation(this.model.userId, productId);
        let productVersionResponse = await new UserService().getProductVersions(this.model.userId, productId);

        if ((versionResponse || limitResponse || productVersionResponse) === ReturnConstraints.REFRESH)
          await this.initializeWithValue(productId);

        this.versionInfos = [];
        this.model.versionInfoIdList = [];

        this.products.forEach((product) => {
          if (product.id === productId) this.selectedProductName = product.name;
        });

        const versionInfos = versionResponse.list;
        versionInfos.forEach((versionInfo) => {
          this.versionInfos.push(versionInfo);
        });
        productVersionResponse.list.forEach((versionInfo) => {
          this.model.versionInfoIdList.push(versionInfo.id);
        });

        if (limitResponse.data !== null) {
          let productLimitation = limitResponse.data;
          this.model.productLimitation = productLimitation;
          if (productLimitation.isDemo) {
            this.dates = [];
            this.dates.push(productLimitation.demoStartDate.substr(0, 10));
            this.dates.push(productLimitation.demoEndDate.substr(0, 10));
          } else this.dates = [];
        }
      }
    },
    async productAdd() {
      let result = await this.$refs.observer.validate();
      if (result) {
        if (this.model.productLimitation.isDemo) {
          const dates = this.dates;
          this.dates = [];
          if (dates[0] < dates[1]) {
            this.model.productLimitation.demoStartDate = new Date(dates[0]);
            this.model.productLimitation.demoEndDate = new Date(dates[1]);
          } else {
            this.model.productLimitation.demoStartDate = new Date(dates[1]);
            this.model.productLimitation.demoEndDate = new Date(dates[0]);
          }
        } else this.dates = [];

        let response = await new UserService().userProductAdd(
          this.model.userId,
          this.model.productId,
          this.model.productLimitation
        );
        if (response.success) {
          await new UserService().userProductAddVersion(
            this.model.userId,
            this.model.productId,
            this.model.versionInfoIdList
          );
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

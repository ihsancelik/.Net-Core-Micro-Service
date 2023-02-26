<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)"><v-icon style="float: right">mdi-close-circle</v-icon></a>
      <h2 class="text-md-center">{{ translator("details") }}</h2>
      <hr class="mb-5" />

      <v-form>
        <ValidationProvider #default="{ errors }" :name="translator('title')" rules="required">
          <v-text-field
            class="field"
            v-model="productDetailModel.title"
            :label="translator('title')"
            :error-messages="errors"
            required
            outlined
          ></v-text-field>
        </ValidationProvider>
        <ValidationProvider #default="{ errors }" :name="translator('content')" rules="required|max:512">
          <v-textarea
            class="field"
            v-model="productDetailModel.content"
            :label="translator('content')"
            :error-messages="errors"
            required
            outlined
            counter="512"
          ></v-textarea>
        </ValidationProvider>

        <hr class="mb-2" />

        <v-btn @click.prevent="productDetailAdd" color="primary" dark class="v-btn--block v-size--large mb-2">
          {{ translator("add") }}
        </v-btn>
      </v-form>
    </v-card>
  </ValidationObserver>
</template>

<script>
import * as rules from "vee-validate/dist/rules";
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import ProductService from "../../../../services/ProductService";

setInteractionMode("eager");
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });
extend("required", { ...rules.required, message: "{_field_} can not be empty" });

export default {
  props: ["productIdProp", "productDetailProp"],
  data() {
    return {
      productId: 0,
      productDetailModel: {},
      signalModel: {
        changes: false,
        returnValues: {},
      },
    };
  },
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  methods: {
    async productDetailAdd() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let response = await new ProductService().addProductDetail(this.productId, this.productDetailModel);
        if (response.success) this.close(true);
      }
    },
    close(changes) {
      this.signalModel.changes = changes;
      this.$emit("closed", this.signalModel);
    },
  },
  created() {
    this.productId = this.productIdProp;
    const detail = this.productDetailProp;
    if (detail !== undefined)
      this.productDetailModel = {
        id: detail.id,
        title: detail.title,
        content: detail.content,
      };
  },
};
</script>

<style scoped>
.field {
  margin-bottom: -4%;
}
</style>

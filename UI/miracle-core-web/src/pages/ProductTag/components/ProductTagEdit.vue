<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)">
        <v-icon style="float: right;">mdi-close-circle</v-icon>
      </a>
      <h2 class="text-md-center">{{ translator("editProductTag") }}</h2>
      <hr class="mb-8" />
      <v-form>
        <ValidationProvider #default="{ errors }" :name="translator('tag')" rules="required|max:32">
          <v-text-field
            v-model="productTagModel.tag"
            :error-messages="errors"
            :label="translator('tag')"
            required
            outlined
            counter="32"
          ></v-text-field>
        </ValidationProvider>

        <hr class="mb-2" />
        <v-btn @click.prevent="productTagEdit" color="primary" dark class="v-btn--block v-size--large mb-2">
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
import * as rules from "vee-validate/dist/rules";
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import MessageBox from "@/helpers/Components/MessageBox";
import ProductTagService from "@/services/ProductTagService";
import { ReturnConstraints } from "@/helpers/Constraints";

setInteractionMode("eager");

extend("required", { ...rules.required, message: "{_field_} can not be empty" });
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });

export default {
  props: ["productTagIdProp"],
  created() {
    this.productTagId = this.productTagIdProp;
    this.initialize();
  },
  data() {
    return {
      productTagModel: {},
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
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  methods: {
    async initialize() {
      let response = await new ProductTagService().getById(this.productTagId);
      if (response === ReturnConstraints.REFRESH) await this.initialize();

      this.productTagModel.id = this.productTagId;
      this.productTagModel = response.data;
    },
    async productTagEdit() {
      let response = await new ProductTagService().edit(this.productTagModel, this.productTagId);
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

<style scoped></style>

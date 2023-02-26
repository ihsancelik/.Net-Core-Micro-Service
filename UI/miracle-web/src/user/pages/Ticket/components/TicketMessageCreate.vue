<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)"><v-icon style="float: right;">mdi-close-circle</v-icon></a>
      <h2 class="text-md-center">{{ translator("createTicket") }}</h2>
      <hr class="mb-3 mt-3" />

      <v-form>
        <ValidationProvider #default="{ errors }" rules="required|max:128">
          <v-select
            v-model="ticketModel.selectedProduct"
            :items="products"
            :placeholder="translator('productOrVersion')"
            :error-messages="errors"
            item-value="value"
            item-text="name"
            outlined
            style="margin-top: 30px;"
          ></v-select>
        </ValidationProvider>

        <ValidationProvider #default="{ errors }" :name="translator('ticketTitle')" rules="required|max:128">
          <v-text-field
            v-model="ticketModel.title"
            :placeholder="translator('ticketTitle')"
            :error-messages="errors"
            solo
          ></v-text-field>
        </ValidationProvider>

        <ValidationProvider #default="{ errors }" :name="translator('message')" rules="required|max:512">
          <v-textarea
            v-model="ticketModel.message"
            :placeholder="translator('message')"
            :error-messages="errors"
            solo
          ></v-textarea>
        </ValidationProvider>

        <v-row cols="12" md="12">
          <v-col cols="12" md="7">
            <v-file-input
              :placeholder="translator('uploadImage')"
              @change="uploadImage"
              accept="image/*"
              prepend-inner-icon="mdi-camera"
              show-size
            ></v-file-input>
          </v-col>
          <v-col cols="12" md="5">
            <img id="sImg" :src="defaultImage" class="img" />
          </v-col>
        </v-row>

        <v-card-actions class="ml-5 mr-5 mt-5">
          <v-btn large outlined color="black" width="100%" @click.prevent="supportCreate">
            {{ translator("send") }}
          </v-btn>
        </v-card-actions>
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
import MessageBox from "@/helpers/components/MessageBox";
import ProductService from "@/services/ProductService";
import TicketService from "@/services/TicketService";
import { ReturnConstraints } from "../../../../helpers/Constraints";

setInteractionMode("eager");
extend("required", { ...rules.required, message: "{_field_} can not be empty" });
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });

export default {
  props: ["groupIdProps"],
  data() {
    return {
      products: [],
      defaultImage: "/miracle-logo.png",
      ticketModel: {
        title: "",
        message: "",
        id: 0,
        selectedProduct: {},
      },
      signalModel: {
        changes: false,
        returnValues: null,
      },
      dialogEnable: false,
      dialogKey: 0,
      dialogWidth: null,
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
      let response = await new ProductService().getListAll();
      if (response === ReturnConstraints.REFRESH) await this.initialize();
      this.products = response.list;
    },
    async supportCreate(e) {
      e.preventDefault();
      let result = await this.$refs.observer.validate();
      if (result) {
        let formData = new FormData();
        for (let [key, value] of Object.entries(this.ticketModel)) {
          formData.append(`${key}`, `${value}`);
        }
        formData.append("ImageName", this.ticketModel.ImageName);

        let response = await new TicketService().create(formData);
        if (response.success) this.close("closed", true);
        else {
          this.model = {};
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
      this.ticketModel.ImageName = image;
      let reader = new FileReader();
      reader.onload = function (e) {
        document.getElementById("sImg").setAttribute("src", e.target.result.toString());
      };
      reader.readAsDataURL(image);
    },
    openDialog(width = 400) {
      this.dialogEnable = true;
      this.dialogWidth = width;
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
    this.ticketModel.id = this.groupIdProps === undefined ? 0 : this.groupIdProps;
    this.initialize();
  },
};
</script>

<style scoped>
.img {
  display: inline-block;
  background-size: contain;
  max-height: 75px;
  height: 75px;
  margin-left: 10%;
}
</style>

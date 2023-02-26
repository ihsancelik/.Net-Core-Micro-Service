<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)"><v-icon style="float: right;">mdi-close-circle</v-icon></a>
      <h2 class="text-md-center">{{ translator("editSlider") }}</h2>
      <hr class="mb-2" />

      <v-form>
        <v-row id="rows1">
          <v-col cols="12" md="8">
            <v-file-input
              prepend-icon=""
              prepend-inner-icon="mdi-camera"
              accept="image/*"
              :placeholder="translator('sliderImage')"
              @change="uploadImage"
            ></v-file-input>
          </v-col>
          <v-col cols="12" md="4">
            <img
              id="vimg"
              src="/miracle-logo.png"
              style="display: inline-block; background-size: contain; max-height: 75px; height: 75px; margin-left: 10%;"
              alt=""
            />
          </v-col>
        </v-row>

        <v-row id="rows">
          <v-col cols="12" md="6">
            <ValidationProvider #default="{ errors }" :name="translator('name')" rules="required">
              <v-text-field
                v-model="slider.name"
                :label="translator('name')"
                :error-messages="errors"
                required
                outlined
                counter="128"
              ></v-text-field>
            </ValidationProvider>
          </v-col>
          <v-col cols="12" md="6">
            <ValidationProvider #default="{ errors }" :name="translator('order')" rules="required">
              <v-text-field
                v-model="slider.order"
                :label="translator('order')"
                :error-messages="errors"
                required
                outlined
                counter="32"
              ></v-text-field>
            </ValidationProvider>
          </v-col>
        </v-row>

        <v-switch v-model="slider.isActive" :label="translator('isActive')" required outlined></v-switch>

        <hr class="mb-2" />

        <v-btn @click.prevent="sliderEdit" color="primary" dark class="v-btn--block v-size--large mb-2">
          {{ translator("update") }}
        </v-btn>
      </v-form>
    </v-card>

    <v-dialog v-model="dialogEnable" max-width="450" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog" />
    </v-dialog>
  </ValidationObserver>
</template>

<script>
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import { Base } from "@/helpers/RouteConstraints";
import * as rules from "vee-validate/dist/rules";
import MessageBox from "@/helpers/components/MessageBox";
import SliderService from "@/services/SliderService";
import { ReturnConstraints } from "../../../../helpers/Constraints";

setInteractionMode("eager");

extend("required", { ...rules.required, message: "{_field_} can not be empty" });

export default {
  props: ["sliderIdProp"],
  data() {
    return {
      slider: {},
      sliderId: null,
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
      let sliderResponse = await new SliderService().getById(this.sliderId);
      if (sliderResponse === ReturnConstraints.REFRESH) await this.initialize();

      this.slider.id = this.sliderId;
      if (sliderResponse) {
        this.slider = sliderResponse.data;

        let imageResponse = await new SliderService().getImage(this.sliderId);
        const img = document.getElementById("vimg");
        img.src = Base + imageResponse;
      }
    },
    async sliderEdit() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let formData = new FormData();

        for (let [key, value] of Object.entries(this.slider)) {
          formData.append(`${key}`, `${value}`);
        }
        formData.append("sliderImage", this.slider.sliderImage);

        let response = await SliderService().edit(formData, this.sliderId);
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
      this.slider.sliderImage = image;
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
  created() {
    this.sliderId = this.sliderIdProp;
    this.initialize();
  },
};
</script>

<style scoped>
#rows {
  margin-bottom: -4%;
}

#rows1 {
  margin-bottom: -3%;
}
</style>

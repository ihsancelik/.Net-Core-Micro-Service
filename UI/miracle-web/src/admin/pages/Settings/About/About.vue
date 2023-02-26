<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5" max-width="50%" style="left: 25%;">
      <v-form>
        <v-row id="rows1">
          <v-col cols="12" md="4">
            <v-file-input
              prepend-inner-icon="mdi-camera"
              accept="image/*"
              :placeholder="translator('aboutImage')"
              @change="uploadImage"
            ></v-file-input>
          </v-col>
          <v-col cols="12" md="8">
            <img
              id="aboutimg"
              :src="about.img"
              style="display: inline-block; background-size: contain; max-height: 200px; height: 200px;"
            />
          </v-col>
        </v-row>
        <ValidationProvider #default="{ errors }" :name="translator('title')" rules="required|max:32">
          <v-text-field
            id="field"
            v-model="about.title"
            :label="translator('title')"
            :error-messages="errors"
            required
            outlined
            counter="32"
          ></v-text-field>
        </ValidationProvider>

        <ValidationProvider #default="{ errors }" :name="translator('text')" rules="required">
          <v-textarea
            id="field"
            v-model="about.text"
            :label="translator('text')"
            :error-messages="errors"
            required
            outlined
            counter="1024"
          ></v-textarea>
        </ValidationProvider>

        <hr class="mb-2" />

        <v-btn @click="aboutUpdate" color="primary" dark class="v-btn--block v-size--large mb-2">
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
import { Base } from "../../../../helpers/RouteConstraints";
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import * as rules from "vee-validate/dist/rules";
import MessageBox from "../../../../helpers/components/MessageBox";
import AboutService from "../../../../services/AboutService";

setInteractionMode("eager");
extend("required", { ...rules.required, message: "{_field_} can not be empty" });
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });
extend("numeric", { ...rules.numeric, message: "{_field_} can be numeric" });

export default {
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  data: () => ({
    paboutService: new AboutService(),
    about: {},
    dialogEnable: false,
    dialogKey: 0,
    selectedComponent: "",
    componentProps: null,
  }),
  methods: {
    initialize() {
      this.paboutService.get().then((response) => {
        if (response === 999) this.initialize();
        this.paboutService.getImage(1).then((img) => {
          const about = response.data;

          about.img = null;
          if (img !== false) about.img = Base + img;
          this.about = about;
        });
      });
    },
    aboutUpdate(e) {
      e.preventDefault();
      this.$refs.observer.validate().then((result) => {
        if (result) {
          let formData = new FormData();

          for (let [key, value] of Object.entries(this.about)) {
            formData.append(`${key}`, `${value}`);
          }
          formData.append("aboutImage", this.about.aboutImage);

          this.paboutService.edit(formData, this.about.id).then((response) => {
            if (response.success) {
              this.selectedComponent = MessageBox;
              this.componentProps = {
                messageTitleProp: this.translator("success"),
                messageTextProp: response.message,
              };
              this.dialogEnable = true;
              this.dialogKey += 1;
            } else {
              this.selectedComponent = MessageBox;
              this.componentProps = {
                messageTitleProp: this.translator("failed"),
                messageTextProp: response.message,
              };
              this.dialogEnable = true;
              this.dialogKey += 1;
            }
          });
        }
      });
    },
    uploadImage(image) {
      this.about.aboutImage = image;
      let reader = new FileReader();
      reader.onload = function (e) {
        document.getElementById("aboutimg").setAttribute("src", e.target.result.toString());
      };
      reader.readAsDataURL(image);
    },
    closeDialog() {
      this.dialogEnable = false;
      this.dialogKey += 1;
    },
  },
  created() {
    this.initialize();
  },
};
</script>

<style>
#aboutimg .v-responsive__content {
  width: 100% !important;
}
</style>

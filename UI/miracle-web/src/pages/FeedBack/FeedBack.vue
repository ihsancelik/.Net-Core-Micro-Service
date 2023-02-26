<template>
  <v-menu :close-on-content-click="false" :value="dialog" :nudge-width="200" offset-y>
    <template v-slot:activator="{ on, attrs }">
      <v-btn @click="dialog = true" icon dark class="ma-2 white--text" color="#767676" v-bind="attrs" v-on="on">
        <v-icon>mdi-emoticon-happy</v-icon>
      </v-btn>
    </template>

    <v-card>
      <v-card-title class="headline" style="margin-left: 19%"> Could You Rate Us </v-card-title>
      <v-card-text>
        <div class="text-center mt-1 ml-1">
          <v-rating
            v-model="rating"
            :items="rating"
            color="yellow darken-3"
            background-color="grey darken-1"
            empty-icon="$ratingFull"
            half-increments
            hover
            large
          ></v-rating>
        </div>
      </v-card-text>

      <v-list style="margin-left: 2%">
        <v-list-item style="height: 100px; margin-left: 6px">
          <v-list-item-action>
            <v-select
              v-model="selectedOption"
              :items="options"
              item-text="name"
              outlined
              placeholder="Öneri/Şikayet"
              style="width: 343px"
            ></v-select>
          </v-list-item-action>
        </v-list-item>

        <v-list-item style="height: 100px; margin-left: 6px">
          <v-list-item-action style="width: 350px">
            <v-select
              v-model="selectedProduct"
              :items="products"
              item-text="name"
              :placeholder="translator('selectProduct')"
              outlined
            ></v-select>
          </v-list-item-action>
        </v-list-item>

        <input type="text" v-model="message" @keyup.enter.enter="send()" placeholder="Rate us.." class="input" />
      </v-list>
      <hr style="width: 83%; margin-left: 37px; margin-top: -48px" />

      <v-card-actions class="justify-space-between">
        <v-btn icon dark text @click="dialog = false" class="noBtn">
          <span class="noBtn"> No Thanks </span>
        </v-btn>
        <v-btn @click="send()" text class="nowBtn"> Rate Now </v-btn>
      </v-card-actions>
    </v-card>
  </v-menu>
</template>

<script>
/* eslint-disable */
import FeedBackService from "@/services/FeedBackService";
import MessageBox from "@/helpers/components/MessageBox";

export default {
  name: "FeedBack",
  props: ["rating", "message", "products", "options", "dialog"],
  data() {
    return {
      model: {},
      selectedOption: "",
      selectedProduct: "",

      dialogKey: 0,
      dialogEnable: false,
      dialogWidth: null,
      selectedComponent: "",
      componentProps: null,
    };
  },
  methods: {
    send() {
      const formData = new FormData();

      formData.append("rate", this.rating);
      formData.append("message", this.message);
      formData.append("options", this.selectedOption);
      formData.append("selectedProduct", this.selectedProduct);

      new FeedBackService().create(formData).then((response) => {
        if (response.success) {
          this.selectedComponent = MessageBox;
          this.componentProps = {
            messageTitleProp: this.translator("success"),
            messageTextProp: this.translator("successfullySent"),
            cancelButtonVisibleProp: false,
          };
          this.dialog = false;
        }
      });
    },
  },
};
</script>

<style scoped>
.noBtn {
  margin-top: 5%;
  margin-left: 9%;
  margin-bottom: 2%;
  color: #0a0a0a;
}
.nowBtn {
  margin-top: 5%;
  margin-bottom: 2%;
  margin-right: 2%;
  color: blue;
}
.input {
  height: 100px;
  margin-left: 20px;
  padding-left: 26px;
}
.v-application--is-ltr .v-list-item__action:first-child,
.v-application--is-ltr .v-list-item__icon:first-child {
  margin-right: 17px !important;
  margin-bottom: -2% !important;
}
</style>
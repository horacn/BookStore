jQuery.validator.methods.date = function (value, element) {
    return this.optional(element) || (/^\d{4}[\/-]\d{1,2}[\/-]\d{1,2}$/.test(value));
}

$(document).ready($(function () {

    $('#assetTypeDialog').dialog({

        autoOpen: false,

        width: 400,

        height: 300,

        modal: true,

        title: 'Add Asset Type',

        buttons: {

            'Save': function () {

                var createAssetTypeForm = $('#createAssetTypeForm');

                if (createAssetTypeForm.valid()) {

                    $.post(createAssetTypeForm.attr('action'), createAssetTypeForm.serialize(), function (data) {

                        if (data.Error != '') {

                            alert(data.Error);

                        }

                        else {

                            // Add the new AssetType to the dropdown list and select it

                            $('#AssetTypeId').append(

                                $('<option></option>')

                                    .val(data.AssetType.AssetTypeId)

                                    .html(data.AssetType.Name)

                                    .prop('selected', true)  // Selects the new AssetType in the DropDown LB

                            );

                            $('#assetTypeDialog').dialog('close');

                        }

                    });

                }

            },

            'Cancel': function () {

                $(this).dialog('close');

            }

        }

    });
    $('#assetTypeAddLink').click(function () {

        var createFormUrl = $().attr('href');

        $('#assetTypeDialog').html('')

            .load(createFormUrl, function () {

                // The createAssetTypeForm is loaded on the fly using jQuery load.

                // In order to have client validation working it is necessary to tell the 

                // jQuery.validator to parse the newly added content

                jQuery.validator.unobtrusive.parse('#createAssetTypeForm');

                $('#assetTypeDialog').dialog('open');

            });

        return false;

    });


}));



terraform {
  backend "s3" {}
}


variable "region" {
  default = "eu-west-1"
}


provider "aws" {
  region = "${var.region}"
}

resource "aws_api_gateway_rest_api" "test_dotnet_api" {
  name        = "do-stuff-api"
  description = "The DoStuff Api"
}

module "booking-api" {
  source           = "github.com/mtranter/terraform-lambda-api-gateway//module"
  source_file      = "./../src/BookingApi.Web/bin/Release/netcoreapp2.0/BookingApi.Web.zip"
  function_name    = "booking-api"
  runtime          = "dotnetcore2.0"
  handler          = "BookingApi.Web::BookingApi.Web.LambdaEntryPoint::FunctionHandlerAsync"
  stage_name       = "prod"
  account_id       = "277618971297"
  rest_api_id      = "${aws_api_gateway_rest_api.test_dotnet_api.id}"
  parent_id        = "${aws_api_gateway_rest_api.test_dotnet_api.root_resource_id}"
  http_method     = "ANY"
  region           = "${var.region}"
  function_timeout = "30"
}


output "invoke_url" {
  value = "${module.booking-api.aws_api_gateway_deployment_invoke_url}"
}

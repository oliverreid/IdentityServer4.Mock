output "invoke_url" {
  value = "${module.booking-api.aws_api_gateway_deployment_invoke_url}"
}

output "test_user_id" {
  value = "${aws_iam_access_key.test_user.id}"
}

output "test_user_secret" {
  value = "${aws_iam_access_key.test_user.secret}"
}
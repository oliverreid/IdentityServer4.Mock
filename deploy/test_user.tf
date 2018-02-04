resource "aws_iam_access_key" "test_user" {
  user    = "${aws_iam_user.test_user.name}"
}

resource "aws_iam_user" "test_user" {
  name = "integration-test-user"
  path = "/booking-api/"
}

resource "aws_iam_user_policy" "test_user_ro" {
  name = "integration-test-user-db-access"
  user = "${aws_iam_user.test_user.name}"

  policy = <<EOF
{
    "Version": "2008-10-17",
    "Statement": [
        {
            "Action": "dynamodb:*",
            "Effect": "Allow",
            "Resource": "${aws_dynamodb_table.events_table.arn}",
            "Sid": ""
        }
    ]
}
EOF
}


resource "aws_dynamodb_table" "events_table" {
  name           = "Events"
  read_capacity  = "${var.db_read_capacity}"
  write_capacity = "${var.db_read_capacity}"
  hash_key       = "EventId"
  range_key      = "ProviderId"

  attribute {
    name = "EventId"
    type = "S"
  }

  attribute {
    name = "ProviderId"
    type = "S"
  }
}

resource "aws_iam_role_policy" "segment-s3-dynamo-lambda" {
  name = "dynamo-policy"
  role = "${module.booking-api.lamdba_role_id}"
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
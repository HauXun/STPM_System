import { TopicService } from "~/app/modules/topic/domain/services/TopicServices";
import { TopicDataSource } from "~/app/modules/topic/infrastructure/data-source/TopicDataSource";
import { TopicRepositoryImpl } from "~/app/modules/topic/infrastructure/repositories/TopicRepositoryImpl";
import { UserService } from "../../user/domain/services/UserServices";
import { UserDataSource } from "../../user/infrastructure/data-source/UserDataSource";
import { UserRepositoryImpl } from "../../user/infrastructure/repositories/UserRepositoryImpl";
import { CommentService } from "../../comment/domain/services/CommentServices";
import { CommentDataSource } from "../../comment/infrastructure/data-source/CommentDataSource";
import { CommentRepositoryImpl } from "../../comment/infrastructure/repositories/CommentRepositoryImpl";
import { PostService } from "../../post/domain/services/PostServices";
import { PostDataSource } from "../../post/infrastructure/data-source/PostDataSource";
import { PostRepositoryImpl } from "../../post/infrastructure/repositories/PostRepositoryImpl";
import { RankAwardService } from "../../rankAward/domain/services/RankAwardServices";
import { RankAwardDataSource } from "../../rankAward/infrastructure/data-source/RankAwardDataSource";
import { RankAwardRepositoryImpl } from "../../rankAward/infrastructure/repositories/TagRepositoryImpl";
import { TagService } from "../../tag/domain/services/TagServices";
import { TagDataSource } from "../../tag/infrastructure/data-source/TagDataSource";
import { TagRepositoryImpl } from "../../tag/infrastructure/repositories/TagRepositoryImpl";

export const defaultTopicService = new TopicService(new TopicRepositoryImpl(new TopicDataSource()));

export const defaultUserService = new UserService(new UserRepositoryImpl(new UserDataSource()));

export const defaultCommentService = new CommentService(new CommentRepositoryImpl(new CommentDataSource()));

export const defaultPostService = new PostService(new PostRepositoryImpl(new PostDataSource()));

export const defaultTagService = new TagService(new TagRepositoryImpl(new TagDataSource()));

export const defaultRankAwardService = new RankAwardService(new RankAwardRepositoryImpl(new RankAwardDataSource()));

export const defaultTopicService = new TopicService(new TopicRepositoryImpl(new TopicDataSource()));
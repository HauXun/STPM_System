import { useState, useEffect } from "react";
import { Post } from "../modules/post/domain/models/Post";
import { selectPostById } from "../modules/post/infrastructure/store/selectors";
import { defaultPostService } from "../modules/shared/common";
import { useAppSelector } from "../stores/hooks";

export const usePostDetails = (postId: string) => {
  const selectPost = selectPostById(postId);
  const postStoreSelect = useAppSelector(selectPost);
  const [post, setPost] = useState<Post | undefined>();

  useEffect(() => {
    if (!postId) return;

    if (postStoreSelect) {
      setPost(postStoreSelect);
      return;
    }

    (async () => {
      try {
        const data = await defaultPostService.getPostsById(postId);
        setPost(data);
      } catch (error) {
        console.log('Failed to fetch post details', error);
      }
    })();
  }, [postId, postStoreSelect]);

  return post;
};